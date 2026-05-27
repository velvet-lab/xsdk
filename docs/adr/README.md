# Architecture Decision Records (ADRs)

This folder contains the Architecture Decision Records for the xSDK project. ADRs document significant architectural decisions made during the development of the framework.

## Format

Each ADR follows this structure:

- **Title**: Short descriptive title
- **Status**: `Proposed` | `Accepted` | `Deprecated` | `Superseded`
- **Context**: Why a decision was needed
- **Decision**: What was decided
- **Consequences**: What results from the decision (positive and negative)

## Index

| ADR                                                       | Title                                                            | Status                         |
|-----------------------------------------------------------|------------------------------------------------------------------|--------------------------------|
| [ADR-001](ADR-001-modular-library-architecture.md)        | Modular Library Architecture                                     | Accepted                       |
| [ADR-002](ADR-002-slim-host-singleton.md)                 | SlimHost as Central Singleton Facade                             | Accepted                       |
| [ADR-003](ADR-003-plugin-extensibility-model.md)          | Plugin-Based Extensibility Model                                 | Accepted                       |
| [ADR-004](ADR-004-variable-setup-configuration-system.md) | Variable/Setup System for Configuration                          | Accepted                       |
| [ADR-005](ADR-005-repository-pattern-with-factory.md)     | Repository Pattern with Factory-Based Initialization             | Accepted                       |
| [ADR-006](ADR-006-provider-agnostic-data-layer.md)        | Provider-Agnostic Data Layer Abstraction                         | Accepted                       |
| [ADR-007](ADR-007-entity-framework-data-provider.md)      | Entity Framework Core as Relational Data Provider                | Accepted                       |
| [ADR-008](ADR-008-nosql-litedb-provider.md)               | LiteDB (LiteDB.Async) as Embedded NoSQL Provider                 | Accepted                       |
| [ADR-009](ADR-009-flatfile-jsonstore-provider.md)         | JsonFlatFileDataStore as Flat-File Provider                      | Accepted                       |
| [ADR-010](ADR-010-vault-secret-management.md)             | HashiCorp Vault as Secret Management Provider                    | Accepted                       |
| [ADR-011](ADR-011-mongodb-via-efcore.md)                  | MongoDB Access via EF Core Provider                              | Accepted                       |
| [ADR-012](ADR-012-demo-fake-repository-mode.md)           | Demo Mode with In-Memory Fake Repository                         | Accepted                       |
| [ADR-013](ADR-013-nlog-logging-framework.md)              | NLog as Logging Framework                                        | Superseded by ADR-014          |
| [ADR-014](ADR-014-opentelemetry-observability.md)         | OpenTelemetry for Observability (Metrics, Tracing, Logging)      | Accepted (extended 2026-03-27) |
| [ADR-015](ADR-015-aspnetcore-web-host-extension.md)       | ASP.NET Core Web Host Extension                                  | Accepted                       |
| [ADR-016](ADR-016-cloudevents-integration.md)             | CloudNative.CloudEvents Integration                              | Accepted                       |
| [ADR-017](ADR-017-spectre-console-commands.md)            | Spectre.Console.Cli for Command-Line Interface                   | Accepted                       |
| [ADR-018](ADR-018-mapster-object-mapping.md)              | Mapster for Object-to-Object Mapping                             | Accepted                       |
| [ADR-019](ADR-019-zio-filesystem-abstraction.md)          | Zio for Cross-Platform File System Abstraction                   | Accepted                       |
| [ADR-020](ADR-020-central-package-management.md)          | Central NuGet Package Management                                 | Accepted                       |
| [ADR-021](ADR-021-semver-version-validation.md)           | Semantic Versioning for Plugin Compatibility                     | Accepted                       |
| [ADR-022](ADR-022-weikio-plugin-framework.md)             | Weikio.PluginFramework for Runtime Plugin Loading                | Accepted                       |
| [ADR-023](ADR-023-aspnetcore-links-hypermedia.md)         | Hypermedia Links Extension for REST APIs                         | Accepted                       |
| [ADR-024](ADR-024-xsdk-core-foundation-layer.md)          | xSdk.Core as Unified Foundation Layer                            | Accepted                       |
| [ADR-025](ADR-025-consul-data-provider.md)                | HashiCorp Consul as Service-Discovery and Configuration Provider | Accepted (think-tank)          |
| [ADR-026](ADR-026-slim-host-builder-redesign.md)          | Slim Host Builder Redesign                                       | Accepted                       |
| [ADR-027](ADR-027-plugin-host-model.md)                   | Plugin Host Model (IPluginHost / WebPluginHost)                  | Accepted                       |
| [ADR-028](ADR-028-database-handler-objectpool.md)         | Database Handler Object Pool                                     | Accepted                       |
| [ADR-029](ADR-029-aspnetcore-security-plugins.md)         | ASP.NET Core Security and Infrastructure Plugins                 | Accepted                       |
| [ADR-030](ADR-030-ai-agents-extension.md)                 | AI Agents Integration as Host Extension                          | Accepted                       |
| [ADR-031](ADR-031-microsoft-testing-platform.md)          | Microsoft Testing Platform for .NET Test Execution               | Accepted                       |
