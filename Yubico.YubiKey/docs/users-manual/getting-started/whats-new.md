<!-- Copyright 2022 Yubico AB

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. -->

# What's new in the SDK?

Here you can find all of the updates and release notes for published versions of the SDK.

## 1.3.x Releases

### 1.3.0

Release date: March 31st, 2022

This release brings enhancements across the SDK.

Features:

- **PIV Objects**. There is now a new namespace, `Yubico.YubiKey.Piv.Objects` that contains high level
  representations of common PIV objects such as [CHUID](xref:Yubico.YubiKey.Piv.Objects.CardholderUniqueId),
  [CCC](xref:Yubico.YubiKey.Piv.Objects.CardCapabilityContainer), and
  [KeyHistory](xref:Yubico.YubiKey.Piv.Objects.KeyHistory). These objects, paired
  with two new methods [ReadObject](xref:Yubico.YubiKey.Piv.PivSession.ReadObject*) and
  [WriteObject](xref:Yubico.YubiKey.Piv.PivSession.WriteObject(Yubico.YubiKey.Piv.Objects.PivDataObject))
  provide a much easier mechanism for interacting with common PIV objects.
- **Direct credential gathering**. Some applications, such as PIV and OATH, require a user to authenticate using
  a PIN or password. The SDK has a robust mechanism called the [KeyCollector](xref:UsersManualKeyCollector)
  for gathering credentials. Supplying a key collector will mean that your application will always be notified
  for the right credential at the right time. Sometimes, though, you may not want to use a key collector, and
  supplying the credential directly to the session is preferable. For this, we've added overloads to
  the most common credential gathering routines (e.g. [TryVerifyPin](xref:Yubico.YubiKey.Piv.PivSession.TryVerifyPin(System.ReadOnlyMemory{System.Byte},System.Nullable{System.Int32}@)))
  that allow you to provide the credential directly, without the need for a key collector.
- **Feature queries**. Rather than keeping track of YubiKey firmware versions and other properties, your
  application can now directly [query a YubiKey](xref:Yubico.YubiKey.YubiKeyFeatureExtensions.HasFeature(Yubico.YubiKey.IYubiKeyDevice,Yubico.YubiKey.YubiKeyFeature))
  to see whether it supports a particular feature.
- **Protected PIV management keys**. Some applications, such as YubiKey Manager or the YubiKey Smart
  Card Mini-Driver, may opt to [only use the PIV PIN](xref:UsersManualPivPinOnlyMode). It does this by
  storing the PIV management key in a PIN protected object and using the PIN to unlock the smart card. The SDK
  has been enlightened to these modes of operations and the PivSession will automatically detect and act
  appropriately. That is, the KeyCollector will automatically ask for a PIN instead of the Management key
  for keys that are configured in this way. No extra handling is required by your application.
- **Yubico.NativeShims**. A new internal-use library has been introduced to help facilitate better
  interoperability with the underlying native platform libraries. No functional changes should have
  occurred as a result of this change. This will instead open the door to broader support of platforms,
  specifically with regards to Linux distributions.


Bug fixes:

- Fixed a high CPU usage issue on Windows that was introduced in 1.2.0. This bug was encountered when multiple
  YubiKeys were plugged into a single computer, and the user reduced the number of keys to one.
- Fixed an issue where the interfaces and applications were not being reported correctly for YubiKey NEOs.

## 1.2.x Releases

### 1.2.0

Release date: February 7th, 2022

This release adds support for device notifications. Now, applications can be notified in real-time that a
YubiKey has been inserted or removed from the computer. Read more about how device notifications work and
how to use them on [this page](xref:DeviceNotifications).

Device notifications are supported on all currently supported platforms.

## 1.1.x Releases

### 1.1.0

Release date: December 3rd, 2021

This release marks the beginning of support for Linux platforms. The primary target for testing has been
against Ubuntu Linux 20.04 LTS and 21.10. Other Ubuntu-based distributions should work as well. Additional Linux
platforms may work based on their [ABI](https://en.wikipedia.org/wiki/Application_binary_interface) compatibility
with Ubuntu. Further distributions will be added to the supported list once thorough testing on those platforms
has been completed.

Limited smart card only support may be present for additional distributions, as they depend on the
PCSC-lite library.

Some symlinks may need to be present in order for the .NET runtime to find the appropriate system
libraries (such as pcsc-lite, udev, etc.) Information about how to create these links can be found
on [this page](xref:YubiKeyTransportHIDKeyboard).

## 1.0.x Releases

### 1.0.2

Release date: October 26th, 2021

Added Authenticode signing to the release process. Assemblies are now signed in addition to the NuGet package.

No code changes in this release.

### 1.0.1

Release date: October 1st, 2021

Bug fixes:
- PIV: Fixed an issue that was preventing the SDK from allowing attestation to occur on certain slots.
- OATH Sample code: Fixed an issue that was causing an exception to be thrown during `RunGetCredentials`.
- PIV Sample code: Worked around an issue in the .NET BCL where certificate generation behavior was different on macOS from Windows.

### 1.0.0

Release date: August 30th, 2021

This is the first official, generally available release of the YubiKey SDK for Desktop aimed at the .NET developer community and
ecosystem. In this release, the [OTP](xref:OtpOverview), [OATH](xref:OathOverview), and [PIV](xref:PivOverview) applications are
fully supported. Please refer to those applications' sections within the documentation for more information.

The [Overview of the SDK](xref:OverviewOfSdk) page also goes into much more depth on what this SDK contains, and how it is structured.

Lastly, this SDK has also been released as open source under the Apache 2.0 license. Please refer to the `CONTRIBUTING.md` file in the
root of the repository for information on how you can contribute.

### 1.0.0-Beta.20210721.1

Release date: July 21st, 2021

This is the beta refresh of the YubiKey Desktop SDK. In this release, the OATH, PIV and OTP applications are now fully supported.
Many OTP features have been completed since the last beta release, we have implemented:

- HOTP
- Challenge-Response with HMAC and Yubico OTP algorithms
- Calculate Challenge-Response with touch notification
- Reading and writing NDEF tags,
- Delete, swap and update OTP slot functionalities.

### 1.0.0-Beta.20210618.1

Release date: June 18th, 2021

This is the first public preview of the new YubiKey Desktop SDK. This SDK allows you to integrate the YubiKey into your .NET
based application or workflow. The OATH and PIV applications are fully supported, with partial support for Yubico OTP. Full
support for Yubico OTP will come in the next beta refresh. There is support for macOS and Windows, over both USB and
Near-Field Communication (NFC).

As the first public beta, the API surface is considered stable. However, if sufficient feedback is received, some minor breaking
changes may occur prior to general availability (GA).

### 1.0.0-Alpha.20210521.1

Release date: May 21st, 2021

This was a limited availability preview.

- A bug was addressed in the smart card reader code which computed an incorrect buffer offset based on the architecture
  of the computer running the YubiKey SDK software.
- OATH functionality is now "feature complete."
- YubiKey device management functionality has been added.

### 1.0.0-Alpha.20210505.1

Release date: May 5th, 2021

This was a limited availability preview.

- PIV functionality is now "feature complete". OATH APIs are partially available.
- A bug was identified and addressed where the default PIV management key could not be used due to a `CryptographicException`
  being thrown by the .NET TripleDES implementation. This is because the default management key is considered a "weak" key.
- A design re-review of the PivSession class identified an over-use of the TryParse pattern. This has been addressed.

Breaking API changes in Yubico.YubiKey:

- Several methods on the `PivSession` class have been renamed as they no longer follow the TryParse pattern.
- `KeyEntryData` and `KeyEntryRequest` have been moved from the `Yubico.YubiKey.Cryptography` namespace to the
  `Yubico.YubiKey` namespace.
- Information previously found in `IYubiKey.DeviceInfo` has been collapsed into the YubiKey object itself by means of
  the `IYubiKeyDeviceInfo` interface.
- Naming of the cryptography delegates have been updated to reflect the .NET Framework Design Guidelines naming conventions.
  For example, `CreateRng` and `CreateTripleDes` have been renamed to `RngCreator` and `TripleDesCreator` respectively.

### 1.0.0-Alpha.20210329.1

Release date: March 29th, 2021

This was a limited availability preview.

- A bug was found and addressed that affected the stability of smart card connections. This would affect any
  command that was sent from the PIV or OATH applications, and would have a higher likelihood of occurring
  for long-running operations. The bug would result in certain method calls failing sporadically.

Breaking API changes in Yubico.YubiKey:

- The `ConnectionType` enum has been renamed to `Transport`
- `YubiKeyEnumerator.GetYubiKeys()` has been replaced by `YubiKey.FindAll()`
- There is no longer a concrete YubiKey instance type. Interaction should be done through the `IYubikey` interface and
  related types.
- Certain constants related to the OTP NDEF "file ID" have been pulled out into an enumeration called `NdefFileId`
- `CreateAttestationCertificateCommand` and `CreateAttestationCertificateResponse` classes have been renamed to
  `CreateAttestationStatementCommand` and `CreateAttestationStatementResponse`, respectively, to reflect the
  terminology already established in published specifications and documentation.

### 1.0.0-Alpha.20210222.1

Release date: February 22nd, 2021

This was a limited availability preview.

- Enumeration of YubiKeys on macOS and Windows platforms
- macOS supports CCID communication only. Windows supports CCID and HID.
- OTP, OATH, PIV, and SCP03 have full low-level command support. All APDUs are mapped to a C# class.
- PIV high level commands are partially implemented. Certificate enrollment scenarios were prioritized.
