# ADR-010: HashiCorp Vault as Secret Management Provider

## Status

Accepted

## Date

2026-03-17

## Context

Enterprise and cloud-native applications require secure management of secrets (API keys, passwords, certificates). Hardcoding secrets in configuration files or environment variables is a security risk. HashiCorp Vault is a widely adopted, open-source secret management platform that provides:

- KV (key-value) secret storage with versioning.
- Multiple authentication methods (AppRole, LDAP, JWT, OIDC, token, certificates, username/password).
- Audit logging.
- Lease-based secrets and dynamic credentials.

The SDK needs to expose Vault as a data provider so that secrets can be read through the same `IRepository` abstraction used for other data stores.

## Decision

`xSdk.Data.Vault` implements a Vault KV v2 provider using the **VaultSharp** NuGet package.

### Authentication Methods

A dedicated class exists for each supported auth method:

| Class                  | Vault Auth Backend                           |
|------------------------|----------------------------------------------|
| `AppRoleAuth`          | AppRole (recommended for machine-to-machine) |
| `UsernamePasswordAuth` | Userpass                                     |
| `LdapAuth`             | LDAP                                         |
| `JwtAuth`              | JWT/OIDC                                     |
| `OidcAuth`             | OIDC with browser redirect                   |
| `TokenAuth`            | Direct token (dev/test only)                 |
| `CertAuth`             | TLS certificate                              |

`VaultAuthenticationMethod` enum controls which auth class is instantiated.

### Components

| Class                     | Responsibility                                                                             |
|---------------------------|--------------------------------------------------------------------------------------------|
| `VaultDatabase`           | Opens and caches `VaultClient` (from VaultSharp); persists connection                      |
| `VaultConnectionBuilder`  | Builds Vault server URL and auth settings from `VaultDatabaseSetup`                        |
| `VaultDatabaseSetup`      | Holds `ServerUrl`, `MountPoint`, `AuthMethod`, auth-specific fields, `PathFormat` delegate |
| `VaultSetup`              | `Setup` derivative exposing Vault config as `Variable`-backed properties                   |
| `ReadOnlyVaultRepository` | Implements `IReadOnlyVaultRepository` — reads KV v2 secrets                                |
| `VaultRepository`         | Extends read-only; adds `AddSecretAsync` for writing secrets                               |

### Path Formatting

`VaultDatabaseSetup.PathFormat` is a `Func<Stage, string, string>` delegate. When set, it is called before every read/write operation to transform the logical path into a stage-specific path (e.g., `/dev/myapp/db-password` vs `/prod/myapp/db-password`). This enables environment-aware secret routing without changes to application code.

```csharp
config.PathFormat = (stage, path) => $"/{stage.ToString().ToLower()}/{path}";
```

### Read Pattern

```csharp
var secrets = await repo.ReadSecretsAsync(mountPoint: null, path: "myapp/dbpassword");
```

`ReadOnlyVaultRepository` calls `VaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint)` and returns `Dictionary<string, object>`.

### Registration

```csharp
builder.UseVault("MyVault", config =>
{
    config.ServerUrl = "https://vault.example.com:8200";
    config.AuthMethod = VaultAuthenticationMethod.AppRole;
    config.AppRoleRoleId = envSetup.VaultRoleId;
    config.AppRoleSecretId = envSetup.VaultSecretId;
    config.PathFormat = (stage, path) => $"/{stage}/{path}";
});
```

## Consequences

### Positive

- Centralized secret management with audit trail (Vault-side).
- Stage-aware path routing prevents cross-environment secret access.
- Multiple auth methods cover all deployment topologies (CI/CD, containers, VMs, developer workstations).
- `IReadOnlyVaultRepository` allows enforcement of read-only access at compile time.

### Negative

- Vault must be running and reachable — any network issue causes the application to fail. Retry/circuit-breaker logic is the application's responsibility.
- The `VaultClient` is cached as a persisted connection; its lease/token may expire during long-running applications without token renewal.
- `PathFormat` is a runtime delegate — incorrect implementations can cause silent routing to wrong environments.
- Vault credential values (`AppRoleRoleId`, `AppRoleSecretId`) must themselves be sourced securely (e.g., from another `Variable` backed by environment variables or a container secret mount).
