// Copyright 2021 Yubico AB
//
// Licensed under the Apache License, Version 2.0 (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Security.Cryptography;
using Yubico.YubiKey.Scp03.Commands;
using Yubico.YubiKey.Scp03;
using Yubico.Core.Iso7816;

namespace Yubico.YubiKey.Pipelines
{
    /// <summary>
    /// Performs SCP03 encrypt-then-MAC on commands and verify-then-decrypt on responses.
    /// </summary>
    /// <remarks>
    /// Does an SCP03 Initialize Update / External Authenticate handshake at setup.
    ///
    /// Commands and responses sent throught this pipeline are confidential and authenticated.
    ///
    /// Requires pre-shared <see cref="StaticKeys"/>.
    /// </remarks>
    internal class Scp03ApduTransform : IApduTransform
    {
        private readonly IApduTransform _pipeline;
        private readonly Session _session;
        private readonly StaticKeys _staticKeys;

        /// <summary>
        /// Constructs a new pipeline from the given one.
        /// </summary>
        /// <param name="pipeline">Underlying pipeline to send and receive encoded APDUs with</param>
        /// <param name="staticKeys">Static keys pre-shared with the device</param>
        public Scp03ApduTransform(IApduTransform pipeline, StaticKeys staticKeys)
        {
            if (pipeline is null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }

            if (staticKeys is null)
            {
                throw new ArgumentNullException(nameof(staticKeys));
            }

            _pipeline = pipeline;
            _staticKeys = staticKeys;

            _session = new Session();
        }

        /// <summary>
        /// Performs SCP03 handshake. Must be called after SELECT.
        /// </summary>
        public void Setup()
        {
            using var rng = RandomNumberGenerator.Create();
            Setup(rng);
        }

        internal void Setup(RandomNumberGenerator rng)
        {
            _pipeline.Setup();
            // Generate host challenge
            byte[] hostChallenge = new byte[8];
            rng.GetBytes(hostChallenge);
            // Perform IU/EA handshake
            PerformInitializeUpdate(hostChallenge);
            PerformExternalAuthenticate();
        }

        public ResponseApdu Invoke(CommandApdu command, Type commandType, Type responseType)
        {
            // Encode command
            CommandApdu encodedCommand = _session.EncodeCommand(command);
            // Pass along the encoded command
            ResponseApdu response = _pipeline.Invoke(encodedCommand, commandType, responseType);
            // Decode response and return it
            return _session.DecodeResponse(response);
        }

        public void Cleanup() => _pipeline.Cleanup();

        private void PerformInitializeUpdate(byte[] hostChallenge)
        {
            InitializeUpdateCommand initializeUpdateCommand = _session.BuildInitializeUpdate(hostChallenge);

            ResponseApdu initializeUpdateResponseApdu = _pipeline.Invoke(
                initializeUpdateCommand.CreateCommandApdu(),
                typeof(InitializeUpdateCommand),
                typeof(InitializeUpdateResponse));

            InitializeUpdateResponse initializeUpdateResponse = initializeUpdateCommand.CreateResponseForApdu(initializeUpdateResponseApdu);
            initializeUpdateResponse.ThrowIfFailed();
            _session.LoadInitializeUpdateResponse(initializeUpdateResponse, _staticKeys);
        }

        private void PerformExternalAuthenticate()
        {
            ExternalAuthenticateCommand externalAuthenticateCommand = _session.BuildExternalAuthenticate();

            ResponseApdu externalAuthenticateResponseApdu = _pipeline.Invoke(
                externalAuthenticateCommand.CreateCommandApdu(),
                typeof(ExternalAuthenticateCommand),
                typeof(ExternalAuthenticateResponse));

            ExternalAuthenticateResponse externalAuthenticateResponse = externalAuthenticateCommand.CreateResponseForApdu(externalAuthenticateResponseApdu);
            externalAuthenticateResponse.ThrowIfFailed();
            _session.LoadExternalAuthenticateResponse(externalAuthenticateResponse);
        }
    }
}
