---
uid: OathOverview
summary: *content
---

<!-- Copyright 2021 Yubico AB

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. -->

# OATH overview

The YubiKey supports the Initiative for Open Authentication (OATH) standards for generating one-time password (OTP) codes. Using this application, a YubiKey can be configured with multiple OTP credentials in a manner similar to that found in software authenticators. Unlike a software only solution, the credentials are stored in the YubiKey itself and can roam between your devices. Simply plug the YubiKey into a device that has YubiKey OATH-enabled software, and your credentials will be there.

The YubiKey-generated one-time passcodes can be used as one of the authentication options in your application's two-factor or multi-factor authentication workflow. A 6 or 8 digit passcode is not strong enough to stand on its own; it is often used to augment the user's primary credentials.

## OTP code

An OTP is similar to a password, however it can only be used once. It is often used in combination with a regular password as an additional authentication mechanism providing extra security.

The OTP code is generated using two inputs: a shared secret and a moving factor. The secret is a static value that is created when you establish a new credential. It is only communicated once, and is stored on both the authenticator (the YubiKey) and the authentication server. The moving factor changes each time a new OTP is requested. The way the moving factor is generated and agreed upon is different for HOTP and TOTP credentials.

### HOTP Credential

HOTP stands for HMAC-based One-time Password algorithm (HOTP). The algorithm is specified in [RFC 4226](https://datatracker.ietf.org/doc/html/rfc4226). 

It is an event-based OTP where the moving factor is based on a counter. Each time the HOTP is requested and validated, the moving factor is incremented based on a counter. The code that is generated is valid until you actively request another one and the authentication server validates it. The OTP generator and the server are synchronized each time the code is validated, and the user gains access.

### TOTP Credential

TOTP stands for Time-based One-time Password (TOTP). The algorithm is specified in [RFC 6238](https://datatracker.ietf.org/doc/html/rfc6238).

The moving factor in a TOTP is time-based rather than counter-based.

The amount of time in which each password is valid is called a timestep. The default timestep is 30 seconds, but it can also be 15 or 60 seconds. If you have not used your password within that window, it will no longer be valid, and you will need to request a new one to gain access to your application.

### HOTP vs. TOTP

HOTP credentials do not have an expiration period. TOTP improves HOTP by using the current time as the moving factor. TOTP credentials have the advantage of being valid for a limited time period — the timestep. So if the generated code is not used within a certain period of seconds, it expires and can not be used for login.

TOTP credentials tend to be more secure because they're only valid for a certain period of time, which adds a certain layer of security. The fact of adding an extra factor that needs to be met increases the security of the code. TOTPs are considered an evolved form of HOTPs— they imply more security because of having an extra factor to meet the algorithm conditions.

Since HOTP doesn’t have the time-based limitation, it’s a little more user-friendly but may be more susceptible to brute force attacks. That’s because of a potentially longer window in which the HOTP is valid.

Also, HOTP can have a synchronization problem. The HOTP counter is incremented each time the button is tapped, while the server counter is incremented only when a password is successfully validated. For this reason, HOTP validating servers accept a range of OTPs. Specifically, they will accept an OTP generated by a counter within a set number of increments from the previous counter value stored on the server. This range is the validation window. If the token counter is outside of the window allowed by the server, the validation fails, and the token must be re-synchronized. The larger the window - the greater the chance of an adversary guessing one of the accepted OTPs through a brute-force attack.

The HOTP algorithm is practical and sound. The possible brute force attack can be prevented by careful implementation of countermeasures in the validation server.

However, given TOTP's advantages over the HOTP protocol, TOTP should be favored whenever possible.

## Use OATH with the YubiKey

When using OATH with a YubiKey, the shared secrets are stored and processed in the YubiKey’s secure element. This has two advantages over software-only solutions:


1. Security

    The secrets always stay within the YubiKey. A phone can get stolen, sold, infected by malware, have its storage read by a connected computer, etc.

2. Accessibility

     You can display OATH codes on more than one phone or computer. If your phone runs out of battery, you can get a code using a friend’s phone or your computer.

## OATH functionality

All OATH operations can be divided into three groups with related types of functionality:

| Common |
| :--- |
| Connect to OATH application |
| Reset OATH application |

| Managing Authentication |
| :--- |
| Set password |
| Validate password |
| Remove password |

| Managing Credentials |
| :--- |
| Get all configured credentials on the YubiKey |
| Calculate OTPs for all configured credentials |
| Calculate OTP for specific credential |
| Add (or overwrite) credential |
| Remove credential |
| Rename credential |

## Next Steps

Read more about [credentials](./oath-credentials.md).

To build an application that utilizes an OATH functionality, the developer can use the SDK. [Make the connection](../sdk-programming-guide/making-a-connection.md), create a
[OathSession](xref:Yubico.YubiKey.Oath.OathSession), then call appropriate methods. The OathSession methods are based on lower level [commands](./oath-commands.md).

Read more about [OATH Session](./oath-session.md) and the main [OATH use case](./oath-use-case.md).

Sample code is also available demonstrating how to use the SDK to perform OATH operations.
