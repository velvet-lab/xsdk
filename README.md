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

![.NET Version](https://img.shields.io/badge/.NET-10.0-512bd4?logo=dotnet)
![pnpm Version](https://img.shields.io/badge/pnpm-10.x-f69220?logo=pnpm)
[![License: Apache-2.0](https://img.shields.io/badge/License-Apache_2.0-yellow.svg)](https://opensource.org/licenses/Apache-2.0)

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
- [Architectural Decision Records](#architectural-decision-records)
- [Contributing](#contributing)
- [Security](#security)
- [License](#license)

---

## About

**xSDK** is a modular SDK library for .NET that provides a collection of reusable building blocks for data access, hosting, extensibility, and observability. It abstracts common infrastructure concerns — such as database providers, file storage, secrets management, and telemetry — behind clean, consistent interfaces, letting application teams focus on domain logic.

Key goals:

- **Pluggable data providers**: Swap between Entity Framework Core, MongoDB, LiteDB, FlatFile, Consul, or Vault-based storage without changing application code.
- **Opinionated hosting**: A slim, convention-based host that wires up DI, logging, and configuration with minimal boilerplate.
- **Extensibility**: Plugin infrastructure and extension points for custom scenarios.
- **Observability**: First-class OpenTelemetry integration for metrics, traces, and logs.

---

## Technology Stack

| Category            | Technology                    | Version    |
|---------------------|-------------------------------|------------|
| Runtime             | .NET                          | 10.0       |
| Language            | C#                            | latest     |
| Web Framework       | ASP.NET Core                  | 10.0       |
| ORM                 | Entity Framework Core         | 10.0.5     |
| Document DB         | MongoDB (EF Core provider)    | 10.0.1     |
| Embedded DB         | LiteDB / LiteDB.Async         | 5.0.21     |
| Flat File Store     | JsonFlatFileDataStore         | 2.4.2      |
| Service Discovery   | Consul                        | 1.8.0      |
| Secrets Management  | VaultSharp                    | 1.17.5.1   |
| Plugin System       | Weikio.PluginFramework        | 1.5.1      |
| Validation          | FluentValidation              | 12.1.1     |
| Object Mapping      | Mapster                       | 10.0.6     |
| Observability       | OpenTelemetry                 | 1.15.1     |
| API Versioning      | Asp.Versioning                | 8.1.1      |
| API Documentation   | Microsoft.AspNetCore.OpenAPI  | 10.0.5     |
| Cloud Events        | CloudNative.CloudEvents       | 2.8.0      |
| CLI                 | Spectre.Console.Cli           | 0.53.1     |
| HTTP Client         | RestSharp                     | 114.0.0    |
| Security Middleware | NWebsec                       | 3.0.0      |
| Serialization       | YamlDotNet                    | 16.3.0     |
| Release Tooling     | semantic-release / pnpm       | —          |

---

## Architecture

xSDK follows a **modular, layered architecture** where each library is independently consumable:

```text
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
│                     xSdk (Host)                          │
│        Hosting | Plugin Extensions | IO | Variables      │
└─────────────────────────┬────────────────────────────────┘
                          │
┌─────────────────────────▼────────────────────────────────┐
│               xSdk.Data (Base Data Layer)                │
│        IDataStore<T> | Repository | Fake Mode            │
└──────┬──────────┬──────────┬──────────┬─────────┬────────┘
       │          │          │          │         │
  ┌────▼───┐ ┌───▼────┐ ┌───▼────┐ ┌───▼──┐ ┌───▼────┐
  │EF Core │ │MongoDB │ │FlatFile│ │Vault │ │Consul  │
  └────────┘ └────────┘ └────────┘ └──────┘ └────────┘
                              │
┌─────────────────────────────▼────────────────────────────┐
│                    xSdk.Core (Foundation)                │
│    Primitives | Utilities | Validation | Mapping | I/O   │
└──────────────────────────────────────────────────────────┘
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

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10) (version `10.0.200` or later, see `global.json`)
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xSdk.Hosting;

var host = Host
    .CreateBuilder(args, "my-app", "my-company", "ma")
    .ConfigureServices((context, services) =>
    {
        services.AddMyService();
    })
    .Build();

await host.RunAsync();
```

---

## Project Structure

```text
xsdk/
├── libs/                               # SDK library projects
│   ├── xSdk.Core/                      # Foundation: primitives, utilities, validation, mapping
│   ├── xSdk/                           # Host layer: hosting, plugin extensions, IO, variables
│   ├── xSdk.Data/                      # Base data layer: IDataStore<T>, repository, fake mode
│   ├── xSdk.Data.Consul/               # Consul service-discovery & config provider
│   ├── xSdk.Data.EntityFramework/      # EF Core data provider
│   ├── xSdk.Data.EntityFramework.MongoDB/  # MongoDB via EF Core provider
│   ├── xSdk.Data.FlatFile/             # JSON flat-file data provider
│   ├── xSdk.Data.NoSql/                # LiteDB embedded NoSQL provider
│   ├── xSdk.Data.Vault/                # HashiCorp Vault secret provider
│   ├── xSdk.Extensions.AspNetCore/     # ASP.NET Core helpers and middleware
│   ├── xSdk.Extensions.AspNetCore.Links/  # HATEOAS-style link generation
│   ├── xSdk.Extensions.CloudEvents/    # CloudNative.CloudEvents integration
│   ├── xSdk.Extensions.Commands/       # CLI command extensions (Spectre.Console)
│   └── xSdk.Extensions.Telemetry/      # OpenTelemetry setup and configuration
├── demos/                              # Sample applications
│   ├── console/                        # Basic console host demo
│   ├── data-consul/                    # Consul service-discovery demo
│   ├── datalayer-entityframework/      # EF Core demo
│   ├── datalayer-flatfile/             # Flat-file storage demo
│   ├── datalayer-mongodb/              # MongoDB demo
│   ├── datalayer-nosql/                # LiteDB demo
│   ├── datalayer-vault/                # Vault demo
│   ├── host/                           # Generic host demo
│   ├── plugin/                         # Plugin system demo
│   ├── telemetry/                      # OpenTelemetry demo
│   └── webapi/                         # ASP.NET Core Web API demo
├── docs/adr/                           # Architectural Decision Records
├── think-tank/                         # Experimental features
├── tools/                              # Build and generator tools
├── Directory.Build.props               # Shared MSBuild properties
├── Directory.Build.targets             # Shared MSBuild targets
├── Directory.Packages.props            # Central NuGet package version management
├── global.json                         # .NET SDK version pin
├── xsdk.sln                            # Main solution file
└── xsdk-demos.sln                      # Demos solution file
```

Each library under `libs/` follows the same layout:

```text
libs/xSdk.SomeLibrary/
├── src/      # Production code
└── tests/    # Unit tests (mirrors src structure)
```

---

## Key Features

### Foundation Layer (`xSdk.Core`)

- Shared primitives, utility types, and base abstractions used across all other libraries
- Object mapping via `Mapster` (compile-time generated, zero-overhead)
- `FluentValidation` registration helpers
- API versioning primitives via `Asp.Versioning`
- SemanticVersioning support
- `RestSharp`-based HTTP client helpers

### Core Hosting (`xSdk`)

- `SlimHost` — lightweight, convention-based .NET generic host with pre-configured logging and DI
- `TestHost` / `TestHostFixture` — helpers for integration tests
- File system abstractions via `Zio`
- Variable/placeholder resolution engine with YAML support
- Plugin extension points via `Weikio.PluginFramework`
- OpenTelemetry integration wired into host startup

### Data Abstractions (`xSdk.Data`)

- Unified `IDataStore<TEntity>` abstraction shared by all providers
- Repository factory and Fake/In-Memory mode for demos and tests
- YAML-based data seeding support

### Data Layer (`xSdk.Data.*`)

- **Entity Framework Core** — SQL databases with full EF feature set, soft-delete support
- **MongoDB** — document store via the official EF Core MongoDB provider
- **LiteDB** — embedded, serverless NoSQL database (async variant included)
- **FlatFile** — JSON file-based storage for simple scenarios
- **Vault** — read secrets and configuration from HashiCorp Vault
- **Consul** — service discovery and key/value configuration from HashiCorp Consul
- Async-first API with `CancellationToken` propagation throughout

### ASP.NET Core Extensions (`xSdk.Extensions.AspNetCore`)

- API versioning configuration
- Problem Details middleware integration
- FluentValidation DI registration
- NWebsec security headers middleware
- API key authentication support

### Observability (`xSdk.Extensions.Telemetry`)

- OpenTelemetry traces, metrics, and logs in one setup call
- OTLP exporter support (Console + gRPC/HTTP)
- Runtime, EF Core, ASP.NET Core, HTTP, gRPC, Redis, and process instrumentation
- Container, host, and OS resource detectors

### Plugin System (`xSdk`)

- Dynamically load and host plugins using `Weikio.PluginFramework`
- Convention-based plugin discovery

### Cloud Events (`xSdk.Extensions.CloudEvents`)

- CloudNative CloudEvents serialization and routing for event-driven architectures

### CLI Commands (`xSdk.Extensions.Commands`)

- Rich CLI application framework via `Spectre.Console.Cli`
- Convention-based command registration

### HATEOAS Links (`xSdk.Extensions.AspNetCore.Links`)

- Hypermedia link generation for REST APIs following HATEOAS principles

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

```text
<type>(<scope>): <description>

feat(data): add LiteDB async support
fix(hosting): resolve null reference in SlimHost startup
docs(readme): update getting started section
```

Common types: `feat`, `fix`, `docs`, `test`, `refactor`, `chore`, `perf`.

### CI/CD Pipelines

| Workflow              | Trigger                       | Description                             |
|-----------------------|-------------------------------|-----------------------------------------|
| `unit-tests.yml`      | Push / PR to `main`, `next`   | Quality assurance — build and test      |
| `sonar-scan.yml`      | PR to `next`                  | SonarCloud quality analysis             |
| `check-format.yml`    | PR to `main`, `next`          | Verify code formatting                  |
| `check-license.yml`   | PR to `main`, `next`          | Validate license headers                |
| `lint-commits.yml`    | PR (opened / sync / edited)   | Conventional commit enforcement         |
| `lint-dotnet.yml`     | PR to `main`, `next`          | .NET code consistency scan              |
| `release.yml`         | Push to `main`, `next`        | Semantic versioning & GitHub Release    |

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

### Frameworks and Libraries

| Library                                  | Version  | Purpose                                                |
|------------------------------------------|----------|--------------------------------------------------------|
| xunit.v3                                 | 3.2.2    | Test runner (`[Fact]`, `[Theory]`)                     |
| Moq                                      | 4.20.72  | Mocking framework                                      |
| Testcontainers.MongoDb                   | 4.11.0   | Integration tests with real containers (MongoDB, etc.) |
| `Xunit.SkippableFact`                    | 1.5.23   | Skip tests conditionally                               |
| `Bogus`                                  | 35.6.5   | Fake data generation                                   |
| `Microsoft.EntityFrameworkCore.InMemory` | 10.0.5   | In-memory EF Core provider for unit tests              |
| `coverlet.collector`                     | 8.0.1    | Code coverage collection (XPlat Code Coverage)         |

### Test Naming

```text
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
    await store.InsertAsync(user);

    var result = await store.SelectAsync("1");

    Assert.NotNull(result);
    Assert.Equal("Alice", result!.Name);
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

## Architectural Decision Records

All significant design decisions are documented in [`docs/adr/`](docs/adr/). Each ADR captures the context, decision, and consequences.

| ADR | Title |
|-----|-------|
| [ADR-001](docs/adr/ADR-001-modular-library-architecture.md) | Modular Library Architecture |
| [ADR-002](docs/adr/ADR-002-slim-host-singleton.md) | Slim Host Singleton |
| [ADR-003](docs/adr/ADR-003-plugin-extensibility-model.md) | Plugin Extensibility Model |
| [ADR-004](docs/adr/ADR-004-variable-setup-configuration-system.md) | Variable / Setup Configuration System |
| [ADR-005](docs/adr/ADR-005-repository-pattern-with-factory.md) | Repository Pattern with Factory |
| [ADR-006](docs/adr/ADR-006-provider-agnostic-data-layer.md) | Provider-Agnostic Data Layer |
| [ADR-007](docs/adr/ADR-007-entity-framework-data-provider.md) | Entity Framework Data Provider |
| [ADR-008](docs/adr/ADR-008-nosql-litedb-provider.md) | NoSQL LiteDB Provider |
| [ADR-009](docs/adr/ADR-009-flatfile-jsonstore-provider.md) | Flat-File JsonStore Provider |
| [ADR-010](docs/adr/ADR-010-vault-secret-management.md) | Vault Secret Management |
| [ADR-011](docs/adr/ADR-011-mongodb-via-efcore.md) | MongoDB via EF Core |
| [ADR-012](docs/adr/ADR-012-demo-fake-repository-mode.md) | Demo Fake Repository Mode |
| [ADR-013](docs/adr/ADR-013-nlog-logging-framework.md) | NLog Logging Framework |
| [ADR-014](docs/adr/ADR-014-opentelemetry-observability.md) | OpenTelemetry Observability |
| [ADR-015](docs/adr/ADR-015-aspnetcore-web-host-extension.md) | ASP.NET Core Web Host Extension |
| [ADR-016](docs/adr/ADR-016-cloudevents-integration.md) | CloudEvents Integration |
| [ADR-017](docs/adr/ADR-017-spectre-console-commands.md) | Spectre Console Commands |
| [ADR-018](docs/adr/ADR-018-mapster-object-mapping.md) | Mapster Object Mapping |
| [ADR-019](docs/adr/ADR-019-zio-filesystem-abstraction.md) | Zio Filesystem Abstraction |
| [ADR-020](docs/adr/ADR-020-central-package-management.md) | Central Package Management |
| [ADR-021](docs/adr/ADR-021-semver-version-validation.md) | SemVer Version Validation |
| [ADR-022](docs/adr/ADR-022-weikio-plugin-framework.md) | Weikio Plugin Framework |
| [ADR-023](docs/adr/ADR-023-aspnetcore-links-hypermedia.md) | ASP.NET Core Links / Hypermedia |
| [ADR-024](docs/adr/ADR-024-xsdk-core-foundation-layer.md) | xSdk.Core as Unified Foundation Layer |
| [ADR-025](docs/adr/ADR-025-consul-data-provider.md) | HashiCorp Consul as Service-Discovery and Configuration Provider |

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

| Resource                                                   | Purpose                                                        |
|------------------------------------------------------------|----------------------------------------------------------------|
| [copilot-instructions.md](.github/copilot-instructions.md) | Project-wide coding context                                    |
| [.github/instructions/](.github/instructions/)             | 9 focused instruction files by topic                           |
| [.github/agents/](.github/agents/)                         | 4 specialized agent modes (see below)                          |
| [.github/skills/](.github/skills/)                         | 10 reusable skill modules from awesome-copilot                 |
| [.github/guidance/](.github/guidance/)                     | Commit, pull request, and review guidance                      |

**Available agents:**

| Agent | Purpose |
|-------|---------|
| **C# Expert** | Senior .NET developer — clean, idiomatic, secure C# code |
| **GitHub Actions Expert** | Secure CI/CD workflows, action SHA pinning, OIDC |
| **ADR Generator** | Create structured Architecture Decision Records |
| **Technical Debt Remediation Plan** | Analyze and prioritize technical debt |

---

## Security

See [SECURITY.md](SECURITY.md) for the full security policy and vulnerability reporting process.

**Supported versions:**

| Version | Supported |
|---------|-----------|
| 1.1.x   | ✅        |
| < 1.1.0 | ❌        |

To report a vulnerability, use [GitHub's private vulnerability reporting](https://github.com/velvet-lab/xsdk/security/advisories/new) or email `danlorb@velvet-lab.net`. **Do not** open public GitHub issues for security vulnerabilities.

---

## License

This project is licensed under the **Apache-2.0 License** — see the [LICENSE](LICENSE) file for details.

Copyright © 2026 [Velvet Lab](https://github.com/velvet-lab)
