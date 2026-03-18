# xSDK Project - GitHub Copilot Instructions

This document provides project-wide coding standards and conventions for the xSDK project. These instructions apply to all code generated or reviewed by GitHub Copilot.

## Project Context

- **Technology Stack**: .NET 10 / C# 13
- **Project Type**: SDK/Library with multiple data layer abstractions and extensions
- **Architecture**: Modular SDK with pluggable data providers and extension points
- **Testing Framework**: xUnit
- **Key Technologies**:
  - ASP.NET Core for web APIs
  - Entity Framework Core for data access
  - OpenTelemetry for observability
  - FluentValidation for validation
  - NLog for logging
  - MongoDB, LiteDB, FlatFile data providers

## Core Principles

1. **Consistency**: Follow existing patterns and conventions in the codebase
2. **Modularity**: Keep components loosely coupled and independently testable
3. **Documentation**: Public APIs must have XML documentation
4. **Testing**: All public APIs require unit tests
5. **Performance**: SDK code should be efficient and avoid unnecessary allocations
6. **Security**: Follow secure coding practices, especially in data access layers

## Project Structure

- `libs/`: Contains all SDK libraries organized by functionality
  - `xSdk/`: Core SDK functionality (hosting, plugin extensions, IO, variables)
  - `xSdk.Data/`: Base data layer abstractions (interfaces, base implementations)
  - `xSdk.Data.*`: Concrete data provider implementations (EntityFramework, EntityFramework.MongoDB, FlatFile, NoSql, Vault)
  - `xSdk.Extensions.*`: Extension libraries for specific scenarios
  - `xSdk.Plugin/`: Plugin infrastructure
- `demos/`: Sample applications demonstrating SDK usage
- `think-tank/`: Experimental features and proof-of-concepts

## Build System

- **Central Package Management**: All package versions are managed in `Directory.Packages.props`
- **Directory.Build.props/targets**: Shared MSBuild configuration across all projects
- **SDK Version**: .NET 10.0.200 (see `global.json`)

## Coding Standards

Refer to the specific instruction files for detailed guidelines:

- [C# Development Guidelines](./.github/instructions/csharp.instructions.md)
- [Testing Best Practices](./.github/instructions/testing.instructions.md)
- [Documentation Standards](./.github/instructions/documentation.instructions.md)
- [Security Guidelines](./.github/instructions/security.instructions.md)
- [Performance Optimization](./.github/instructions/performance.instructions.md)
- [Code Review Standards](./.github/instructions/code-review.instructions.md)

## Workflow and Development

When working on this project:

1. **Before implementing**: Consider using the [Architecture Planning prompt](./.github/prompts/setup-component.prompt.md)
2. **For new features**: Review [implementation best practices](./.github/instructions/csharp.instructions.md)
3. **For testing**: Use the [test generation prompt](./.github/prompts/write-tests.prompt.md)
4. **For refactoring**: Consult the [refactoring prompt](./.github/prompts/refactor-code.prompt.md)
5. **For code review**: Follow the [code review prompt](./.github/prompts/code-review.prompt.md)

## Specialized Modes

Use these specialized agents for specific tasks:

- [Architect Mode](./.github/agents/architect.agent.md) - For architecture and design decisions
- [Reviewer Mode](./.github/agents/reviewer.agent.md) - For thorough code reviews
- [Debugger Mode](./.github/agents/debugger.agent.md) - For debugging issues

## Important Project Conventions

1. **Namespace Structure**: Follow the folder structure (`xSdk.Data.EntityFramework`, etc.)
2. **Async/Await**: All I/O operations must be async
3. **Cancellation Tokens**: Pass through `CancellationToken` in async methods
4. **Nullable Reference Types**: Enabled project-wide - handle nullability correctly
5. **Interface Naming**: Prefix with "I" (e.g., `IDataStore`)
6. **Test Projects**: Named `[ProjectName].Tests` and mirror source structure

## Documentation Requirements

All public APIs must include:
- XML documentation comments (`///`)
- `<summary>` describing what the API does
- `<param>` for each parameter
- `<returns>` describing the return value
- `<example>` with code samples for complex APIs
- `<exception>` for thrown exceptions

## Testing Requirements

- All public APIs must have unit tests
- Follow the Arrange-Act-Assert (AAA) pattern
- Test method naming: `MethodName_Scenario_ExpectedBehavior`
- No Act/Arrange/Assert comments in tests
- Use xUnit assertions (`Assert.*`) as the standard
- Mock external dependencies, not domain logic

## Security Considerations

- Never commit secrets, API keys, or connection strings
- Use configuration providers for sensitive data
- Validate all user inputs
- Use parameterized queries (EF Core does this automatically)
- Follow OWASP security guidelines

## Performance Guidelines

- Avoid unnecessary allocations in hot paths
- Use `Span<T>` and `Memory<T>` when appropriate
- Stream large data instead of loading into memory
- Use appropriate EF Core query patterns (avoid N+1)
- Implement proper disposal patterns (`IDisposable`, `IAsyncDisposable`)

## Architecture Decision Records (ADRs)

All significant architectural decisions are documented in [`docs/adr/`](./docs/adr/README.md). **Before implementing a new feature or changing an existing pattern, consult the relevant ADR(s).** Each ADR documents the rationale, known limitations, and trade-offs of the chosen design.

| Component Area | ADR |
|---|---|
| Modular library structure | [ADR-001](./docs/adr/ADR-001-modular-library-architecture.md) |
| SlimHost singleton facade | [ADR-002](./docs/adr/ADR-002-slim-host-singleton.md) |
| Plugin extensibility model | [ADR-003](./docs/adr/ADR-003-plugin-extensibility-model.md) |
| Variable / Setup configuration | [ADR-004](./docs/adr/ADR-004-variable-setup-configuration-system.md) |
| Repository pattern + factory DI | [ADR-005](./docs/adr/ADR-005-repository-pattern-with-factory.md) |
| Provider-agnostic data layer | [ADR-006](./docs/adr/ADR-006-provider-agnostic-data-layer.md) |
| EF Core data provider | [ADR-007](./docs/adr/ADR-007-entity-framework-data-provider.md) |
| LiteDB NoSQL provider | [ADR-008](./docs/adr/ADR-008-nosql-litedb-provider.md) |
| FlatFile JSON provider | [ADR-009](./docs/adr/ADR-009-flatfile-jsonstore-provider.md) |
| HashiCorp Vault provider | [ADR-010](./docs/adr/ADR-010-vault-secret-management.md) |
| MongoDB via EF Core | [ADR-011](./docs/adr/ADR-011-mongodb-via-efcore.md) |
| Demo / fake repository mode | [ADR-012](./docs/adr/ADR-012-demo-fake-repository-mode.md) |
| NLog logging framework | [ADR-013](./docs/adr/ADR-013-nlog-logging-framework.md) |
| OpenTelemetry observability | [ADR-014](./docs/adr/ADR-014-opentelemetry-observability.md) |
| ASP.NET Core web host | [ADR-015](./docs/adr/ADR-015-aspnetcore-web-host-extension.md) |
| CloudEvents integration | [ADR-016](./docs/adr/ADR-016-cloudevents-integration.md) |
| Spectre.Console CLI commands | [ADR-017](./docs/adr/ADR-017-spectre-console-commands.md) |
| Mapster object mapping | [ADR-018](./docs/adr/ADR-018-mapster-object-mapping.md) |
| Zio file system abstraction | [ADR-019](./docs/adr/ADR-019-zio-filesystem-abstraction.md) |
| Central NuGet package management | [ADR-020](./docs/adr/ADR-020-central-package-management.md) |
| SemVer plugin compatibility | [ADR-021](./docs/adr/ADR-021-semver-version-validation.md) |
| Weikio plugin framework | [ADR-022](./docs/adr/ADR-022-weikio-plugin-framework.md) |
| HATEOAS hypermedia links | [ADR-023](./docs/adr/ADR-023-aspnetcore-links-hypermedia.md) |

## Continuous Improvement

This is a living document. When you notice patterns that should be documented, suggest updates to improve these guidelines.
