# xSDK

> A modular .NET SDK with pluggable data providers, extensible hosting, and built-in observability — designed for building production-ready services and applications.

[![Build Status](https://github.com/velvet-lab/xsdk/actions/workflows/release.yml/badge.svg)](https://github.com/velvet-lab/xsdk/actions)
[![Latest Release](https://img.shields.io/github/v/release/velvet-lab/xsdk?logo=github)](https://github.com/velvet-lab/xsdk/releases)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)
![Unit Tests](https://github.com/velvet-lab/xsdk/actions/workflows/unit-tests.yml/badge.svg)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=coverage)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)

![Security Policy](https://img.shields.io/badge/Security-Policy_Available-success.svg)
[![Dependabot Status](https://img.shields.io/badge/dependabot-enabled-blue.svg?logo=dependabot)](https://github.com/velvet-lab/xsdk/network/updates)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=bugs)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=velvet-lab_xsdk&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=velvet-lab_xsdk)

![.NET Version](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)
![pnpm Version](https://img.shields.io/badge/pnpm-10.x-f69220?logo=pnpm)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## Table of Contents

- [About](#about)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Key Features](#key-features)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Testing](#testing)
- [Contributing](#contributing)
- [Security](#security)
- [License](#license)

---

## About

**xSDK** is a modular SDK library for .NET that provides a collection of reusable building blocks for data access, hosting, extensibility, and observability. It abstracts common infrastructure concerns — such as database providers, file storage, secrets management, and telemetry — behind clean, consistent interfaces, letting application teams focus on domain logic.

Key goals:

- **Pluggable data providers**: Swap between Entity Framework Core, MongoDB, LiteDB, FlatFile, or Vault-based storage without changing application code.
- **Opinionated hosting**: A slim, convention-based host that wires up DI, logging, and configuration with minimal boilerplate.
- **Extensibility**: Plugin infrastructure and extension points for custom scenarios.
- **Observability**: First-class OpenTelemetry integration for metrics, traces, and logs.

---

## Technology Stack

| Category            | Technology                 | Version                      |
|---------------------|----------------------------|------------------------------|
| Runtime             | .NET                       | 8.0 / 9.0                    |
| Language            | C#                         | 12 (latest)                  |
| Web Framework       | ASP.NET Core               | 9.0                          |
| ORM                 | Entity Framework Core      | 9.0.11                       |
| Document DB         | MongoDB (EF Core provider) | 9.0.3                        |
| Embedded DB         | LiteDB / LiteDB.Async      | 5.0.21                       |
| Flat File Store     | JsonFlatFileDataStore      | 2.4.2                        |
| Secrets Management  | VaultSharp                 | 1.17.5.1                     |
| Plugin System       | Weikio.PluginFramework     | 1.5.1                        |
| Validation          | FluentValidation           | 12.1.1                       |
| Object Mapping      | Mapster                    | 7.4.0                        |
| Logging             | NLog                       | 6.0.6                        |
| Observability       | OpenTelemetry              | 1.9.0 (net8) / 1.14.0 (net9) |
| API Versioning      | Asp.Versioning             | 8.1.0                        |
| API Documentation   | Swashbuckle (OpenAPI)      | 9.0.6                        |
| Cloud Events        | CloudNative.CloudEvents    | 2.8.0                        |
| CLI                 | Spectre.Console.Cli        | 0.53.1                       |
| HTTP Client         | RestSharp                  | 113.0.0                      |
| Security Middleware | NWebsec                    | 3.0.0                        |
| Release Tooling     | semantic-release / pnpm    | —                            |

---

## Architecture

xSDK follows a **modular, layered architecture** where each library is independently consumable:

```
┌──────────────────────────────────────────────────────────┐
│                     Application Layer                    │
│         (demos/, ASP.NET Core APIs, Console Apps)        │
└─────────────────────────┬────────────────────────────────┘
                          │
┌─────────────────────────▼────────────────────────────────┐
│                  xSdk.Extensions.*                       │
│   AspNetCore | AspNetCore.Links | CloudEvents |          │
│   Commands   | Telemetry                                 │
└─────────────────────────┬────────────────────────────────┘
                          │
┌─────────────────────────▼────────────────────────────────┐
│                     xSdk (Core)                          │
│        Hosting | Plugin Extensions | IO | Variables      │
└──────┬────────────┬───────────┬────────────┬─────────────┘
       │            │           │            │
 ┌─────▼──┐  ┌─────▼───┐ ┌────▼────┐ ┌─────▼──────┐
 │EF Core │  │MongoDB  │ │FlatFile │ │   Vault    │
 │xSdk    │  │xSdk     │ │xSdk     │ │   xSdk     │
 │.Data.  │  │.Data.   │ │.Data.   │ │   .Data.   │
 │Entity- │  │Entity-  │ │FlatFile │ │   Vault    │
 │Frame-  │  │Frame-   │ └─────────┘ └────────────┘
 │work    │  │work.    │
 └────────┘  │MongoDB  │
             └─────────┘
```

**Core design principles:**

1. **Repository Pattern**: All data access is abstracted behind `IDataStore<TEntity>` interfaces.
2. **Dependency Injection First**: Services register themselves via `Add[ServiceName]` extension methods.
3. **Async/Await Throughout**: All I/O operations are fully async with `CancellationToken` propagation.
4. **Pluggable Providers**: Switch data backends through configuration, not code changes.
5. **Observable by Design**: OpenTelemetry instrumentation built into data and hosting layers.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8) or [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9) (version `8.0.411` or later, see `global.json`)
- [just](https://just.systems/) (command runner for development tasks)
- [pnpm](https://pnpm.io/) 10.x (required for release tooling only)
- [Git](https://git-scm.com/)

### Clone and Build

```bash
git clone https://github.com/velvet-lab/xsdk.git
cd xsdk

# Restore and build all libraries
just build
```

### Run Tests

```bash
just test
```

### Run a Demo

```bash
# Example: console host demo
cd demos/console/host
dotnet run
```

### Using a Library in Your Project

```bash
# Core SDK
dotnet add package xSdk

# Entity Framework data provider
dotnet add package xSdk.Data.EntityFramework

# ASP.NET Core extensions
dotnet add package xSdk.Extensions.AspNetCore
```

### Minimal Host Setup

```csharp
using xSdk.Hosting;

var host = await SlimHost.CreateAsync(args, builder =>
{
    builder.Services.AddMyService();
});

await host.RunAsync();
```

---

## Project Structure

```
xsdk/
├── libs/                          # SDK library projects
│   ├── xSdk/                      # Core SDK (hosting, plugin extensions, IO)
│   ├── xSdk.Data.EntityFramework/ # EF Core data provider
│   ├── xSdk.Data.EntityFramework.MongoDB/  # MongoDB via EF Core
│   ├── xSdk.Data.FlatFile/        # JSON flat-file data provider
│   ├── xSdk.Data.NoSql/           # LiteDB NoSQL provider
│   ├── xSdk.Data.Vault/           # HashiCorp Vault secret provider
│   ├── xSdk.Extensions.AspNetCore/        # ASP.NET Core helpers
│   ├── xSdk.Extensions.AspNetCore.Links/  # HATEOAS-style link generation
│   ├── xSdk.Extensions.CloudEvents/       # CloudNative.CloudEvents integration
│   ├── xSdk.Extensions.Commands/          # CLI command extensions
│   ├── xSdk.Extensions.Telemetry/         # OpenTelemetry setup
│   └── xSdk.Plugin/               # Plugin infrastructure
├── demos/                         # Sample applications
│   ├── console/                   # Basic console host demo
│   ├── datalayer-entityframework/ # EF Core demo
│   ├── datalayer-flatfile/        # Flat-file storage demo
│   ├── datalayer-mongodb/         # MongoDB demo
│   ├── datalayer-nosql/           # LiteDB demo
│   ├── datalayer-vault/           # Vault demo
│   ├── host/                      # Generic host demo
│   ├── plugin/                    # Plugin system demo
│   ├── telemetry/                 # OpenTelemetry demo
│   └── webapi/                    # ASP.NET Core Web API demo
├── think-tank/                    # Experimental features (Consul, Mermaid, Package)
├── tools/                         # Build and generator tools
├── Directory.Build.props          # Shared MSBuild properties
├── Directory.Build.targets        # Shared MSBuild targets
├── Directory.Packages.props       # Central NuGet package version management
├── global.json                    # .NET SDK version pin
├── xsdk.sln                       # Main solution file
└── xsdk-demos.sln                 # Demos solution file
```

Each library under `libs/` follows the same layout:

```
libs/xSdk.SomeLibrary/
├── src/      # Production code
└── tests/    # Unit tests (mirrors src structure)
```

---

## Key Features

### Core Hosting (`xSdk`)

- `SlimHost` — lightweight, convention-based .NET generic host with pre-configured logging and DI
- `TestHost` / `TestHostFixture` — helpers for integration tests
- File system abstractions via `Zio`
- Variable/placeholder resolution engine
- Plugin extension points

### Data Layer (`xSdk.Data.*`)

- Unified `IDataStore<TEntity>` abstraction across all providers
- **Entity Framework Core** — SQL databases with full EF feature set
- **MongoDB** — document store via the official EF Core MongoDB provider
- **LiteDB** — embedded, serverless NoSQL database (async variant included)
- **FlatFile** — JSON file-based storage for simple scenarios
- **Vault** — read secrets and configuration from HashiCorp Vault
- Async-first API with `CancellationToken` propagation throughout

### ASP.NET Core Extensions (`xSdk.Extensions.AspNetCore`)

- API versioning configuration
- Problem Details middleware integration
- FluentValidation-to-Swagger bridge via `MicroElements.Swashbuckle.FluentValidation`
- NWebsec security headers middleware
- API key authentication support

### Observability (`xSdk.Extensions.Telemetry`)

- OpenTelemetry traces, metrics, and logs in one setup call
- OTLP exporter support
- Runtime, EF Core, and process instrumentation

### Plugin System (`xSdk.Plugin`)

- Dynamically load and host plugins using `Weikio.PluginFramework`
- Convention-based plugin discovery

### Cloud Events (`xSdk.Extensions.CloudEvents`)

- CloudNative CloudEvents serialization and routing for event-driven architectures

---

## Development Workflow

### Branching Strategy

| Branch      | Purpose                                  |
|-------------|------------------------------------------|
| `main`      | Stable, released code                    |
| `next`      | Default integration branch (pre-release) |
| `feature-*` | Feature development                      |

Pull requests target the `next` branch. Merges to `main` trigger official releases.

### Commit Messages

This project uses [Conventional Commits](https://www.conventionalcommits.org/) enforced by `lint-commits` CI workflow. Commit messages must follow the format:

```
<type>(<scope>): <description>

feat(data): add LiteDB async support
fix(hosting): resolve null reference in SlimHost startup
docs(readme): update getting started section
```

Common types: `feat`, `fix`, `docs`, `test`, `refactor`, `chore`, `perf`.

### CI/CD Pipelines

| Workflow           | Trigger                     | Description                          |
|--------------------|-----------------------------|--------------------------------------|
| `unit-tests.yml`   | Push / PR to `main`, `next` | Build and run all unit tests         |
| `sonar-scan.yml`   | PR to `main`, `next`        | SonarCloud quality analysis          |
| `mega-linter.yml`  | Push / PR                   | Lint all file types                  |
| `check-format.yml` | Push / PR                   | Verify code formatting               |
| `lint-commits.yml` | PR                          | Conventional commit enforcement      |
| `release.yml`      | Push to `main`, `next`      | Semantic versioning & GitHub Release |

### Release Process

Releases are fully automated via [semantic-release](https://github.com/semantic-release/semantic-release):

```bash
# Install Node.js dependencies (for release tooling)
pnpm install

# Dry-run to preview the next release
pnpm exec semantic-release --dry-run
```

The version is derived from conventional commit history and NuGet packages are published automatically.

---

## Coding Standards

All C# code in this repository follows the guidelines in [.github/instructions/csharp.instructions.md](.github/instructions/csharp.instructions.md). Key conventions:

### Naming

| Construct                          | Convention     | Example                     |
|------------------------------------|----------------|-----------------------------|
| Types, Methods, Properties         | PascalCase     | `DataStore`, `GetByIdAsync` |
| Private fields, locals, parameters | camelCase      | `_repository`, `userId`     |
| Interfaces                         | `I` prefix     | `IDataStore<T>`             |
| Generic type parameters            | `T` prefix     | `TEntity`, `TContext`       |
| Async methods                      | `Async` suffix | `SaveAsync`, `GetAllAsync`  |

### Key Rules

- **Nullable reference types** are enabled project-wide — handle `null` correctly at API boundaries.
- **`ConfigureAwait(false)`** must be used in all library-level async code.
- **`CancellationToken`** must be accepted and passed through in every async method.
- **One type per file**, file name matches the type name.
- **`ArgumentNullException.ThrowIfNull()`** for parameter validation at public entry points.
- Line length target: 120 characters.
- Code formatting is enforced via `.editorconfig` and checked in CI.

### Architecture Patterns

- Register services via `Add[ServiceName]` extension methods.
- Prefer constructor injection; avoid service locator pattern.
- Use the Options pattern (`IOptions<T>`) for configuration.
- Follow the repository pattern for all data access.

For full details see the instruction files in [.github/instructions/](.github/instructions/).

---

## Testing

Testing follows the guidelines in [.github/instructions/testing.instructions.md](.github/instructions/testing.instructions.md).

### Frameworks and Libraries

| Library                                  | Purpose                                                |
|------------------------------------------|--------------------------------------------------------|
| xUnit                                    | Test runner (`[Fact]`, `[Theory]`)                     |
| FluentAssertions                         | Expressive assertions                                  |
| Moq                                      | Mocking framework                                      |
| Testcontainers                           | Integration tests with real containers (MongoDB, etc.) |
| `Xunit.SkippableFact`                    | Skip tests conditionally                               |
| `Bogus`                                  | Fake data generation                                   |
| `Microsoft.EntityFrameworkCore.InMemory` | In-memory EF Core provider for unit tests              |

### Test Naming

```
MethodName_Scenario_ExpectedBehavior
```

Examples:
- `GetByIdAsync_WhenEntityExists_ReturnsEntity`
- `SaveAsync_WithNullEntity_ThrowsArgumentNullException`
- `UpdateAsync_WhenConcurrencyConflict_ThrowsDbUpdateConcurrencyException`

### Structure (AAA Pattern)

```csharp
[Fact]
public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
{
    var store = new InMemoryDataStore<User>();
    var user = new User { Id = "1", Name = "Alice" };
    await store.AddAsync(user);

    var result = await store.GetByIdAsync("1");

    result.Should().NotBeNull();
    result!.Name.Should().Be("Alice");
}
```

### Running Tests

```bash
# All tests
just test

# Specific project
dotnet test libs/xSdk/tests/

# With coverage
dotnet test xsdk.sln --collect:"XPlat Code Coverage"
```

Test projects are named `[ProjectName].Tests` and mirror the source project structure. Tests must be independent and runnable in any order.

---

## Contributing

1. **Fork** the repository and create a feature branch from `next`.
2. Follow the [C# development guidelines](.github/instructions/csharp.instructions.md).
3. Write **unit tests** for all public APIs (mandatory).
4. Ensure all XML documentation (`///`) is complete on public members.
5. Run `just build` and `just test` locally before pushing.
6. Use [Conventional Commits](https://www.conventionalcommits.org/) for all commit messages.
7. Open a pull request targeting the `next` branch.

### Copilot / AI Guidelines

This repository ships with a full set of GitHub Copilot customizations:

| Resource                                                   | Purpose                                                 |
|------------------------------------------------------------|---------------------------------------------------------|
| [copilot-instructions.md](.github/copilot-instructions.md) | Project-wide coding context                             |
| [.github/instructions/](.github/instructions/)             | Focused instruction files by topic                      |
| [.github/prompts/](.github/prompts/)                       | Reusable prompt templates                               |
| [.github/agents/](.github/agents/)                         | Specialized agent modes (Architect, Reviewer, Debugger) |

Use the **Architect** agent for design decisions, **Reviewer** for code reviews, and **Debugger** for diagnosing issues.

---

## Security

See [SECURITY.md](SECURITY.md) for the full security policy and vulnerability reporting process.

**Supported versions:**

| Version | Supported |
|---------|-----------|
| 1.1.x   | ✅         |
| < 1.1.0 | ❌         |

To report a vulnerability, use [GitHub's private vulnerability reporting](https://github.com/velvet-lab/xsdk/security/advisories/new) or email `danlorb@velvet-lab.net`. **Do not** open public GitHub issues for security vulnerabilities.

---

## License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

Copyright © 2026 [Velvet Lab](https://github.com/velvet-lab)
