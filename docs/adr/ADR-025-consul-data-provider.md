# ADR-025: HashiCorp Consul as Service-Discovery and Configuration Provider

## Status

Accepted (implementation in think-tank — not yet promoted to production `libs/`)

## Date

2026-04-04

## Implementation Note (2026-04-27)

`xSdk.Data.Consul` resides in `think-tank/libs/xSdk.Data.Consul/` and has not yet been promoted to the production `libs/` folder. The design described in this ADR reflects the intended architecture. The package will be added to `libs/` and released as a NuGet package once stabilized.

## Context

Cloud-native and microservice applications frequently require:

1. **Service discovery** — locating other services by name rather than hard-coded addresses (e.g., `my-api` resolves to `http://10.0.1.5:8080` at runtime via a registry).
2. **Distributed key/value configuration** — storing and reading runtime configuration from a shared, versioned store without redeploying services (e.g., feature flags, connection strings, tuning parameters).
3. **Health checking** — registering services with liveness/readiness probes so that the registry can route traffic only to healthy instances.

The SDK already provides HashiCorp Vault for secret management ([ADR-010](ADR-010-vault-secret-management.md)), which handles sensitive credentials. However, Vault is not designed for service discovery or general-purpose configuration distribution. A complementary tool is needed.

**HashiCorp Consul** (open-source) is the de-facto standard for service discovery in HashiCorp-based stacks and is widely used alongside Vault in production deployments. It provides:

- A networked key/value store accessible via HTTP API.
- A service catalog with health-check-aware DNS and HTTP-based service discovery.
- ACL-based access control compatible with Vault's auth model.
- Lightweight agent architecture that works in Docker, Kubernetes, and VM environments.

The alternative considered was **Microsoft.Extensions.ServiceDiscovery** (the .NET Aspire-based service discovery abstraction). It was not selected because:

- It requires .NET Aspire orchestration or a custom provider implementation.
- The project does not use .NET Aspire hosting and has an existing HashiCorp toolchain (Vault).

## Decision

`xSdk.Data.Consul` provides a Consul-backed data provider that integrates with the SDK's existing data layer abstraction ([ADR-006](ADR-006-provider-agnostic-data-layer.md)).

### Abstractions Provided

`xSdk.Data.Consul` exposes:

| Type | Role |
|---|---|
| `ConsulEntity` | Base entity type for Consul KV-backed items |
| `ConsulPrimaryKey` | Primary key implementation using Consul KV path segments |
| `ConsulDatabase` / `ConsulDatabaseSetup` | Database context and setup options for Consul connection |
| `ConsulRepository<TEntity>` | `IRepository<TEntity>` implementation backed by Consul KV; supports Insert, Select, Update, Remove, Upsert operations |
| `ConsulConnectionBuilder` | Fluent builder for Consul connection strings (address, datacenter, token) |
| `IConsulService` / `ConsulService` | Higher-level service for Consul service registry interactions (registration, deregistration, health checks) |
| `DatalayerBuilderExtensions` | `AddConsul(...)` extension method on `IDatalayerBuilder` |
| `ServiceCollectionExtensions` | `AddConsulService(...)` for registering `IConsulService` via DI |

### Integration Pattern

Registration follows the same `IDatalayerBuilder` pattern as all other data providers:

```csharp
builder.ConfigureServices((context, services) =>
{
    services.AddDatalayer(datalayer =>
    {
        datalayer.AddConsul(consul =>
        {
            consul.Address = "http://localhost:8500";
            consul.Token   = Environment.GetEnvironmentVariable("CONSUL_TOKEN");
        });
    });
});
```

### KV Repository Usage

The `ConsulRepository<TEntity>` maps an entity's primary key to a Consul KV path and serializes the entity body as JSON:

```
Key path:  /{prefix}/{entity-type}/{primary-key}
Value:     JSON-serialized entity
```

This enables treating Consul's KV store as a lightweight, distributed document store — suitable for small configuration objects, feature flags, or reference data that must be shared across instances.

### Service Discovery Usage

`IConsulService` provides service registration and lookup independent of the repository layer:

```csharp
await consulService.RegisterAsync(new ServiceRegistration
{
    Name        = "my-api",
    Address     = "10.0.1.5",
    Port        = 8080,
    HealthCheck = new HealthCheckConfig { HttpEndpoint = "/health", Interval = "10s" }
});
```

### Dependency

`xSdk.Data.Consul` depends on:

- `xSdk.Data` (base data layer abstractions)
- `Consul 1.8.0` (official HashiCorp Consul .NET client)

## Consequences

### Positive

- **POS-001**: Consul and Vault complement each other — Vault for secrets, Consul for service discovery and configuration, following the established HashiCorp stack pattern.
- **POS-002**: Service discovery removes hard-coded service addresses from configuration, enabling zero-downtime deployments and horizontal scaling.
- **POS-003**: The `IRepository<TEntity>` abstraction means application code using Consul KV is portable — it can be swapped with FlatFile or other providers without code changes.
- **POS-004**: Consul's KV store is suitable for small, frequently-read configuration objects that do not contain secrets.

### Negative

- **NEG-001**: Requires a running Consul agent/cluster; local development without Docker adds an infrastructure dependency.
- **NEG-002**: Consul KV is not a relational or document database; complex query patterns (filtering, aggregation) are not supported.
- **NEG-003**: Consul ACL token management must be handled out-of-band (typically via Vault's Consul secrets engine); the SDK does not automate this.
- **NEG-004**: `ConsulServiceOld.cs` in the current codebase indicates a service discovery API refactor is in progress; the final API surface may change.

## Implementation Notes

- **IMP-001**: Consul connection settings are exposed via `ConsulDatabaseSetup` which implements `IDatabaseSetup`; this integrates with the Variable/Setup system ([ADR-004](ADR-004-variable-setup-configuration-system.md)).
- **IMP-002**: `ConsulRepository<TEntity>` implements all five CRUD partials as separate partial class files (`*.Insert.cs`, `*.Select.cs`, `*.Update.cs`, `*.Remove.cs`, `*.Upsert.cs`) following the same pattern as other SDK repositories.
- **IMP-003**: A demo application exists at `demos/data-consul/host` showing integration with the host layer and shared entity models.
- **IMP-004**: Integration tests use `Testcontainers` with the official Consul Docker image following the same pattern as the MongoDB tests.

## References

- **REF-001**: [ADR-006](ADR-006-provider-agnostic-data-layer.md) — Provider-Agnostic Data Layer Abstraction
- **REF-002**: [ADR-010](ADR-010-vault-secret-management.md) — HashiCorp Vault as Secret Management Provider
- **REF-003**: [ADR-005](ADR-005-repository-pattern-with-factory.md) — Repository Pattern with Factory-Based Initialization
- **REF-004**: [ADR-004](ADR-004-variable-setup-configuration-system.md) — Variable/Setup Configuration System
- **REF-005**: [HashiCorp Consul .NET Client](https://github.com/G-Research/consuldotnet) — `Consul 1.8.0` NuGet package
