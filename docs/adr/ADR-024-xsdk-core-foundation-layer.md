# ADR-024: xSdk.Core as Unified Foundation Layer

## Status

Accepted

## Date

2026-04-04

## Context

The original modular architecture ([ADR-001](ADR-001-modular-library-architecture.md)) designated `xSdk.Plugin` as the cross-cutting primitives package containing all shared abstractions (`ISlimHost`, `SlimHostBase`, `SlimHostBuilder`, `SdkException`, `Stage`, `SemVer`). All other libraries depended on it to prevent circular references.

Over time, the scope of shared abstractions grew substantially beyond plugin primitives:

- Data layer interfaces (`IEntity`, `IModel`, `IPrimaryKey`, `IRepository<TEntity>`, `IDatabase`, `IDatalayerFactory`)
- File system abstractions (`IFileSystemService`, `FileSystemContext`)
- Variable and setup system (`IVariable`, `ISetup`, `IVariableService`, `VariableAttribute`)
- Authentication primitives (`IApiKeyHandler`, `IApiKeyModel`, `ClaimsPrincipalExtensions`)
- ASP.NET abstraction interfaces (`IWebApiPluginBuilder`, `IWebSecurityPluginBuilder`, `ILinksService`, `IHateoasItem`)
- Telemetry interfaces (`ITelemetryService`, `ITelemetryPluginBuilder`)
- Cryptography and security utilities (`CryptoTool`, `SecurityContext`, `CredentialManager`)
- General utilities (`StringHelper`, `HashTools`, `NetworkTools`, `TypeConverter`, `ObjectHelper`)

The name `xSdk.Plugin` no longer accurately described the package's contents and confused consumers who expected a plugin-specific package rather than a general-purpose foundation library.

## Decision

`xSdk.Plugin` is renamed to **`xSdk.Core`** to accurately reflect its role as the foundation layer of the entire SDK.

### Responsibilities of xSdk.Core

`xSdk.Core` is the lowest-level package in the SDK dependency graph. It contains:

| Category           | Key Types                                                                                    |
|--------------------|----------------------------------------------------------------------------------------------|
| Host Abstractions  | `ISlimHost`, `SlimHostBase`, `SlimHostBuilder`, `IPluginHost<TSetup>`                        |
| Plugin Primitives  | `IPlugin`, `IPluginBuilder`, `IPluginDescription`, `IPluginService`                          |
| Data Abstractions  | `IEntity`, `IModel`, `IPrimaryKey`, `IRepository<TEntity>`, `IDatabase`, `IDatalayerFactory` |
| Configuration      | `ISetup`, `IVariable`, `IVariableService`, `VariableAttribute`, `SetupLoader`                |
| File System        | `IFileSystemService`, `FileSystemContext`, `FileSystemHelper`                                |
| Authentication     | `IApiKeyHandler`, `IApiKeyModel`, `AuthenticationDefaults`                                   |
| Web Abstractions   | `IWebApiPluginBuilder`, `IWebSecurityPluginBuilder`, `ILinksService`                         |
| Telemetry          | `ITelemetryService`, `ITelemetryPluginBuilder`                                               |
| Security Utilities | `CryptoTool`, `SecurityContext`, `CredentialManager`, `CertificateHelper`                    |
| Shared SDK Types   | `SdkException`, `Stage`, `SemVer`, `Mapster` mapping base, REST client helpers               |

### Dependency Rule

`xSdk.Core` has deliberately minimal runtime dependencies. It must **not** depend on:

- `Microsoft.EntityFrameworkCore` or any data provider
- `Microsoft.AspNetCore.*` host/runtime packages
- Any external storage client (MongoDB driver, LiteDB, etc.)

This constraint ensures that external plugins can reference `xSdk.Core` without pulling in host-runtime or storage dependencies.

**Actual runtime dependencies (as of 2026-05-27):**

| Package | Reason |
|---------|--------|
| `FluentValidation` | Validation primitives used across core abstractions |
| `Microsoft.Extensions.Hosting` | `IHostBuilder`/`IHost` base abstractions for `SlimHostBuilder` |
| `Zio` | Cross-platform file system abstraction ([ADR-019](ADR-019-zio-filesystem-abstraction.md)) |
| `CommunityToolkit.Diagnostics` | Guard/argument validation helpers |
| `Bogus` | Fake data generation for demo/fake repository mode ([ADR-012](ADR-012-demo-fake-repository-mode.md)) |

> **Note (2026-05-27):** The original decision listed `Mapster`, `RestSharp`, `Asp.Versioning.Http`, and `Weikio.PluginFramework.Abstractions` as the intended minimal set. The actual csproj evolved to include `Microsoft.Extensions.Hosting`, `Zio`, `CommunityToolkit.Diagnostics`, and `Bogus` as the SDK stabilized. The prohibition on EF Core, ASP.NET Core runtime packages, and storage clients remains strictly enforced.

### Migration from xSdk.Plugin

The rename is a breaking change at the NuGet package level:

- `xSdk.Plugin` NuGet package is retired; consumers upgrade to `xSdk.Core`.
- Assembly-level type namespaces are preserved to minimize binary-level breaks.
- `InternalsVisibleTo` declarations in `Directory.Build.props` are updated automatically.

## Consequences

### Positive

- **POS-001**: Package name accurately reflects purpose — consumers understand that `xSdk.Core` is a general foundation, not plugin-specific infrastructure.
- **POS-002**: Single, authoritative location for all cross-cutting abstractions prevents duplication across libraries.
- **POS-003**: External plugin assemblies continue to depend only on this one package, keeping their transitive dependency footprint small.
- **POS-004**: Future abstractions (e.g., new provider interfaces) have a clear home without needing a new package.

### Negative

- **NEG-001**: Breaking NuGet package rename requires consumers to update their package references from `xSdk.Plugin` to `xSdk.Core`.
- **NEG-002**: The package contains a wide surface area; a focused sub-package split (e.g., `xSdk.Core.Data`, `xSdk.Core.Security`) may be needed if size becomes a concern.
- **NEG-003**: Generated build artifacts (`xSdk.Plugin.AssemblyInfo.cs`) may appear in `xSdk.Core` outputs until MSBuild cache is cleared, which can confuse tooling.

## Implementation Notes

- **IMP-001**: All internal project references previously pointing to `xSdk.Plugin` are updated to `xSdk.Core` in their `.csproj` files as part of the rename.
- **IMP-002**: The `csproj` description field must be updated to `xSdk.Core - Foundation layer for the xSdk project`. As of 2026-05-27 it still reads `xSdk.Plugin - Plugin framework extensions for the xSdk project.` — this is a known tracking issue.
- **IMP-003**: ADR-001 package table is updated to replace `xSdk.Plugin` with `xSdk.Core` (see [ADR-001](ADR-001-modular-library-architecture.md)).
- **IMP-004**: ADR-002 references to `xSdk.Plugin` as the host for `ISlimHost` are updated to `xSdk.Core`.
- **IMP-005**: The actual runtime dependencies should be reviewed periodically against the constraint in the Dependency Rule section. `Bogus` in particular should be evaluated for extraction to a separate `xSdk.Core.Testing` or `xSdk.Testing` package to keep production builds lean.

## References

- **REF-001**: [ADR-001](ADR-001-modular-library-architecture.md) — Modular Library Architecture (updated)
- **REF-002**: [ADR-002](ADR-002-slim-host-singleton.md) — SlimHost Singleton (updated)
- **REF-003**: [ADR-003](ADR-003-plugin-extensibility-model.md) — Plugin Extensibility Model
- **REF-004**: [ADR-006](ADR-006-provider-agnostic-data-layer.md) — Provider-Agnostic Data Layer
