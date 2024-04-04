# Payload-Encoder

Payload-Encoder is a simple yet effective tool designed for cybersecurity enthusiasts and practitioners. It's developed as part of a personal project to explore and demonstrate the execution of encoded payloads, particularly focusing on shellcode execution within .NET applications. The tool leverages custom encoding techniques to evade detection and includes methods for AMSI bypass, aiming to provide educational insight into cybersecurity mechanisms and practices.

## Introduction

Created to learn and share knowledge within the cybersecurity community, Payload-Encoder uses modified concepts from cybersecurity courses to execute payloads, specifically targeting Windows environments. It is a practical application of encoding payloads to understand better and demonstrate evasion techniques against security detections.

## Key Features

- Shellcode Execution: Executes shellcode from embedded resources within .NET applications, demonstrating a practical use case for custom encoded payloads.
- Custom Encoding Schemes: This technique employs custom encoding techniques to help evade signature-based detection systems, showcasing an essential aspect of cybersecurity research.
- AMSI Bypass Techniques: This section features methods to bypass AMSI (Antimalware Scan Interface), allowing payloads to be executed without being hindered by security checks.
- Evasion Demonstrations: This section provides examples of evading sandbox and other detection mechanisms, contributing to the knowledge of security evasion strategies.

## Getting Started

### Prerequisites

- Windows 10/11 environment.
- .NET Framework 4.7.2 or newer.
- Visual Studio 2019 or newer for development and compilation.

## Setup

1. Generate Your Payload: First, generate your shellcode payload using a tool like the Sliver C2 framework or Metasploit. This will be your test payload for execution.
2. Encode Your Payload: Use any encoding technique to encode your beacon.bin payload or C# byte array. This project includes examples of such encoding.
3. Embed and Compile: Include your encoded payload as an embedded resource in the project, then compile using Visual Studio.

## Usage

Execute the compiled application, choosing whether to encode or decode your payload based on the options provided. Follow the on-screen prompts to complete your desired action.

## Disclaimer
Payload-Encoder is intended for educational and ethical use only. Always ensure you have permission to test these techniques on any system. This project is meant to share knowledge and inspire safe and ethical exploration in cybersecurity.

***This project is in active development, and currently ingests binary file outputs from C2's or others and C# byte array outputs from the Metasploit Framework `-f csharp` format.***

## License

This project is open-source and licensed under the MIT License. See the LICENSE file for more details.

## Acknowledgments

A big thank you to the cybersecurity community for the ongoing exchange of knowledge and to all those dedicated to ethical hacking and security research. Your contributions make projects like this possible.
