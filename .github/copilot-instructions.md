# xSDK Project - GitHub Copilot Instructions

This document provides project-wide coding standards and conventions for the xSDK project. These instructions apply to all code generated or reviewed by GitHub Copilot.

## Project Context

- **Technology Stack**: .NET 8 / C# 12
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
  - `xSdk/`: Core SDK functionality
  - `xSdk.Data.*`: Data layer abstractions and providers
  - `xSdk.Extensions.*`: Extension libraries for specific scenarios
  - `xSdk.Plugin/`: Plugin infrastructure
- `demos/`: Sample applications demonstrating SDK usage
- `think-tank/`: Experimental features and proof-of-concepts

## Build System

- **Central Package Management**: All package versions are managed in `Directory.Packages.props`
- **Directory.Build.props/targets**: Shared MSBuild configuration across all projects
- **SDK Version**: .NET 8.0.411 (see `global.json`)

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
- Use FluentAssertions when available in test projects
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

## Continuous Improvement

This is a living document. When you notice patterns that should be documented, suggest updates to improve these guidelines.
