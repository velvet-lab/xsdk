---
title: "ADR-029: ASP.NET Core Security and Infrastructure Plugins"
status: "Accepted"
date: "2026-04-27"
authors: "xSdk Team"
tags: ["architecture", "security", "aspnetcore", "authentication", "dataprotection"]
supersedes: ""
superseded_by: ""
---

# ADR-029: ASP.NET Core Security and Infrastructure Plugins

## Status

Accepted

## Date

2026-04-27

## Context

`xSdk.Extensions.AspNetCore` provides a `WebHost` builder ([ADR-015](ADR-015-aspnetcore-web-host-extension.md)) that wires up an ASP.NET Core application. Web API applications require a number of cross-cutting security and infrastructure concerns that do not belong to a single business feature:

- **Authentication** — verifying caller identity (API key, JWT, custom schemes).
- **Authorization** — enforcing access policies on controllers and endpoints.
- **Data Protection** — encrypting cookies, CSRF tokens, and form payloads using ASP.NET Core's `IDataProtectionProvider`.
- **Web Security** — CORS policies, HSTS headers, forwarded headers, and content security headers.
- **Response Compression** — Brotli and GZip compression for API responses.
- **API Versioning and Documentation** — Swagger/OpenAPI endpoint documentation.

Rather than hard-coding these concerns in `WebHost`, they are exposed as opt-in **plugin hosts** following the `IPluginHost` model ([ADR-027](ADR-027-plugin-host-model.md)). This keeps the web host generic and lets individual applications choose which security features to activate.

## Decision

`xSdk.Extensions.AspNetCore` provides four security and infrastructure plugin hosts in the `xSdk.Plugins.*` namespace:

### 1. AuthenticationPluginHost

```csharp
internal sealed class AuthenticationPluginHost(
    IOptions<ApiKeyOptions> apiKeyOptions,
    IOptions<EnvironmentOptions> environmentOptions) : WebPluginHost
```

**Activation:** `builder.EnableAuthentication()` or `builder.EnableAuthentication<TBuilder>()`

**Behavior:**
- Registers `IAuthenticationService` with a `PolicyScheme` (`MultiAuth`) as the default scheme, allowing multiple authentication schemes to coexist.
- Always adds **API key authentication** (`AspNetCore.Authentication.ApiKey`) as the baseline scheme via `authBuilder.AddApiKeyAuth(...)`.
- Calls `InvokeBuilders<IAuthenticationPluginBuilder>` to allow extensions to add additional schemes (e.g., JWT Bearer, certificate).
- Registers default authorization with `RequireAuthenticatedUser` as the default policy.

**Extensibility:** Implement `IAuthenticationPluginBuilder` and register via `builder.RegisterPluginBuilder<IAuthenticationPluginBuilder, TImpl>()`.

**Security note:** API key values are validated via `IApiKeyHandler`. The default `DefaultApiKeyHandler` resolves keys from the registered `IApiKeyModel` store. Custom key storage (e.g., Vault-backed) is plugged in by implementing `IApiKeyHandler`.

### 2. DataProtectionPluginHost

```csharp
public sealed class DataProtectionPluginHost(
    IOptions<ApplicationOptions> applicationOptions,
    IOptions<DataProtectionOptions> pluginOptions) : PluginHost
```

**Activation:** `builder.EnableDataProtection()`

**Behavior:**
- Calls `services.AddDataProtection()` with the application name as discriminator (preventing key-ring conflicts between applications sharing a machine).
- Applies configurable key lifetime from `DataProtectionOptions.KeyLifetime` (parsed as `TimeSpan`).
- Calls `InvokeBuilder<IDataProtectionPluginBuilder>` to allow extensions to configure the key storage location (e.g., file system, Azure Key Vault, Vault).

**Extensibility:** Implement `IDataProtectionPluginBuilder` and register via `builder.RegisterPluginBuilder<IDataProtectionPluginBuilder, TImpl>()`.

### 3. WebSecurityPluginHost

```csharp
internal sealed class WebSecurityPluginHost(
    IOptions<WebSecurityOptions> websecurityOptions,
    IOptions<EnvironmentOptions> environmentOptions) : WebPluginHost
```

**Activation:** `builder.EnableWebSecurity()`

**Behavior:**
- **CORS** — adds a default policy restricting origins to the configured allow-list (`WebSecurityOptions.AllowedOrigins`). CORS is only enabled when `WebSecurityOptions.IsCorsEnabled == true`.
- **HSTS** — enforces `Strict-Transport-Security` with 365-day `max-age`, `includeSubDomains`, and `preload` flags.
- **Forwarded Headers** — configures `X-Forwarded-For` and `X-Forwarded-Proto` handling for reverse-proxy deployments.
- **Security Headers middleware** — sets `X-Content-Type-Options: nosniff`, `X-Frame-Options`, and `Referrer-Policy` headers.
- Calls `InvokeBuilder<IWebSecurityPluginBuilder>` to allow additional middleware registration.

### 4. CompressionPluginHost

```csharp
internal sealed class CompressionPluginHost : PluginHost
```

**Activation:** `builder.EnableCompression()`

**Behavior:**
- Registers `AddResponseCompression` with both **Brotli** (`BrotliCompressionProvider`) and **GZip** (`GzipCompressionProvider`) providers.
- Sets `EnableForHttps = true` — compression is applied even on HTTPS responses (acceptable for API-only endpoints where BREACH-style attacks are not a concern).

### Configuration via Options

Each plugin host reads its settings through the standard `IOptions<T>` pattern (wired via `RegisterPluginHostOptions<T>()`):

| Plugin         | Options Class           | Key Variables                                                                  |
|----------------|-------------------------|--------------------------------------------------------------------------------|
| Authentication | `ApiKeyOptions`         | `{prefix}_APIKEY_HEADER`, `{prefix}_APIKEY_VALUE`                              |
| DataProtection | `DataProtectionOptions` | `{prefix}_DATAPROTECTION_DISCRIMINATOR`, `{prefix}_DATAPROTECTION_KEYLIFETIME` |
| WebSecurity    | `WebSecurityOptions`    | `{prefix}_WEBSECURITY_CORS_ENABLE`, `{prefix}_WEBSECURITY_ORIGINS`             |

### API Key Model Abstraction

`IApiKeyModel` and `IApiKeyHandler` (defined in `xSdk.Core`) decouple the authentication middleware from the key storage:

```csharp
public interface IApiKeyHandler
{
    Task<IApiKeyModel?> ValidateAsync(string apiKey, CancellationToken cancellationToken);
}
```

The `DefaultApiKeyHandler` reads configured keys from `ApiKeyOptions`. Applications can replace this with a database-backed or Vault-backed implementation by registering `IApiKeyHandler` in DI before calling `EnableAuthentication()`.

## Consequences

### Positive

- **POS-001**: Security and infrastructure features are strictly opt-in — applications that do not call `EnableAuthentication()` have zero authentication middleware overhead.
- **POS-002**: All security configuration is driven by the `Variable`/`Setup` system ([ADR-004](ADR-004-variable-setup-configuration-system.md)), enabling environment-variable or secrets-file configuration without code changes.
- **POS-003**: The `IAuthenticationPluginBuilder` / `IDataProtectionPluginBuilder` / `IWebSecurityPluginBuilder` extension points allow downstream libraries to add new schemes without modifying the core plugin hosts.
- **POS-004**: `IApiKeyHandler` abstraction keeps the API key validation logic swappable — Vault-backed keys, database-backed keys, or static keys are all supported via DI replacement.

### Negative

- **NEG-001**: CORS `EnableForHttps = true` on `CompressionPluginHost` may expose APIs to BREACH-style compression oracle attacks if responses contain secret user-specific content. This is acceptable for pure JSON APIs but must be reviewed for cookie/session-bearing APIs.
- **NEG-002**: The `MultiAuth` policy scheme adds one extra middleware round-trip for every request to select the active authentication handler.
- **NEG-003**: `DataProtectionPluginHost` is a `PluginHost` (not `WebPluginHost`) and therefore cannot register ASP.NET Core middleware directly; it relies on `services.AddDataProtection()` which is middleware-agnostic by design.

## Alternatives Considered

##### Hard-code Security in WebHost.Services.cs

- **ALT-001**: **Description**: Move all security registration into `WebHost.Application.cs` unconditionally.
- **ALT-002**: **Rejection Reason**: Forces every application to carry authentication, CORS, HSTS, and compression even when not needed (e.g., internal gRPC services, test hosts). Opt-in plugins avoid this overhead.

##### Use Dedicated NuGet Packages per Security Feature

- **ALT-003**: **Description**: Split authentication, data protection, web security, and compression into separate `xSdk.Extensions.AspNetCore.Auth`, `xSdk.Extensions.AspNetCore.Security`, etc.
- **ALT-004**: **Rejection Reason**: These features are tightly coupled to ASP.NET Core and are almost always used together in a web API host. Keeping them in one package avoids dependency graph fragmentation while still allowing individual activation via separate `Enable*()` calls.

## Implementation Notes

- **IMP-001**: `AuthenticationPluginHost` uses `AddPolicyScheme(AuthenticationDefaults.MulitAuth.Scheme, ...)` as the default scheme. The `EnableMultiAuth` callback selects the concrete scheme per request (API key, JWT, etc.) based on the presence of specific request headers.
- **IMP-002**: `WebSecurityPluginHost.Configure()` calls `app.UseForwardedHeaders()` *before* `UseAuthentication()` so that `X-Forwarded-For` is correctly resolved for rate-limiting and audit logging.
- **IMP-003**: Compression must be activated *before* the MVC middleware via `app.UseResponseCompression()`. `CompressionPluginHost`'s `Configure` method places this call in the correct middleware order relative to other plugin hosts, ordered by `PluginDescription.Order`.

## References

- **REF-001**: [ADR-015](ADR-015-aspnetcore-web-host-extension.md) — ASP.NET Core Web Host Extension
- **REF-002**: [ADR-027](ADR-027-plugin-host-model.md) — Plugin host model used by all four plugin hosts
- **REF-003**: [ADR-004](ADR-004-variable-setup-configuration-system.md) — Variable/Setup system used for security option configuration
- **REF-004**: [OWASP BREACH](https://owasp.org/www-community/attacks/BREACH_Attack) — Compression oracle attack reference
- **REF-005**: [ASP.NET Core Data Protection](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction)
