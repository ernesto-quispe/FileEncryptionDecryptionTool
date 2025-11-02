# Overview

I developed this file encryption and decryption application to explore practical uses of cryptographic algorithms within the language. This project implements AES-256 based encryption to handle real-world file security challenges.

My purpose was to strengthen practical knowledge of C# language constructs and cryptography libraries by building a functional and secure application.

[Software Demo Video](https://www.youtube.com/watch?v=5jKBFHWv1_A)

# Development Environment

- Developed with Visual Studio Code editor, using C# extensions.
- Language: C# targeting .NET 7 SDK.
- Cryptography: System.Security.Cryptography namespace for AES.
- Build and run environment: .NET CLI (dotnet run, dotnet build).

# Useful Websites

- [Microsoft Learn - Cryptographic Services](https://learn.microsoft.com/en-us/dotnet/standard/security/cryptographic-services)
- [Stack Overflow - AES Encryption in C#](https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp)

# Future Work

- Implement secure external key management instead of embedding key and IV in files.
- Add password-based encryption/decryption using key derivation (PBKDF2).
- Enhance error handling for corrupted files and invalid formats.
- Support recursive encryption/decryption for folders and display progress.
- Develop a graphical user interface (GUI) to improve usability.
