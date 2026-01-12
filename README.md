# Lithium CLR

**Lithium CLR** is a C# rewrite of the Hytale server, replacing Java with a high-performance, Common Language Runtime (CLR) designed for scalability, stability, and modern tooling.

## Features

- **Full C# Rewrite**: Complete server reimplementation originally in Java, now in C#.
- **Performance Focused**: Reduced memory allocations and high-speed packet handling.
- **Plugin-Friendly**: Designed to allow extensions and custom plugins.
- **Cross-Platform**: Works on Windows, Linux, and macOS.

## Development Roadmap

Here is the current development roadmap, with progress tracked using checkboxes:

- [x] **Experiment with UDP/QUIC networking**  
  - Testing packet transmission, latency, and reliability.  
- [ ] **Define server architecture**  
  - Decide on module separation, packet handling, threading, and memory management.  
- [ ] **Server interpretation (Java → C#)**  
  - Once the official Hytale Java server is available, analyze and interpret the logic in C#.  
- [ ] **Zero-cost optimization rewrite**  
  - Rewrite core systems for minimal allocations, maximum speed, and low memory footprint.  
- [ ] **Advanced features**  
  - Dashboard for server management (player tracking, logs, plugins).  
  - Plugin system with event handling and safe execution.  
  - High-performance networking and multithreading.  
- [ ] **Testing & stability**  
  - Load testing, memory profiling, and stress tests.  
- [ ] **Documentation & community tools**  
  - API references, plugin guides, and tutorials.

> The roadmap is subject to change as experimentation progresses and the official Java server becomes available.

## Installation

1. Clone the repository:

```bash
git clone https://github.com/lithium-clr/lithium-server.git
```

## Aspire

We use Aspire to build and run the server.
Aspire is great because it provides a cross-platform, unified development experience, code-first configuration, and local orchestration (which is really important when developing mods with local servers).

Aspire Docs: https://aspire.dev/docs/

Aspire is also backed by the .NET Foundation, and is a growing project with a lot of contributors (https://github.com/dotnet/aspire).

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Aspire CLI](https://aspire.dev/get-started/install-cli/)
- Docker Engine (Docker Desktop for Windows/Mac, or Docker Engine for Linux)

### Using Aspire

To run the server, use the following command from the root of the repository:
```bash
aspire run
```
You should see Aspire starting up, if everything is working correctly, it should ouput an url for the Aspire Dashboard. You can open this in your browser to see each resource states.