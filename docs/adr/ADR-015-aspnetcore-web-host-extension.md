# ADR-015: ASP.NET Core Web Host Extension

## Status

Accepted

## Date

2026-03-17

## Context

The core `xSdk.Hosting.Host` provides a generic host. Web API applications additionally need:

- Kestrel (or IIS) configuration.
- MVC/controller pipeline setup.
- Security headers and middleware.
- HTTPS, CORS, and API versioning.
- Content root management (different between demo and production modes).
- Stage-aware error handling (detailed errors in Development, suppressed in Production).

Rather than forcing every web application to configure all of this from scratch, the SDK should provide a `WebHost` builder that layeres web concerns on top of the generic `Host`.

## Decision

`xSdk.Extensions.AspNetCore` provides `WebHost.CreateBuilder` which extends `xSdk.Hosting.Host.CreateBuilder`.

### WebHost.CreateBuilder

```csharp
var builder = xSdk.Hosting.Host.CreateBuilder(args, appName, appCompany, appPrefix)
    .ConfigureWebHostDefaults(webHostBuilder =>
    {
        webHostBuilder
            .UseContentRoot(contentRoot)
            .UseWebRoot(contentRoot)
            .UseEnvironment(stage.ToString())
            .UseSetting(WebHostDefaults.DetailedErrorsKey, (stage == Stage.Development).ToString())
            .ConfigureServices(ConfigureWebHostServicesWithContext)
            .Configure(ConfigureApplicationWithContext)
            .UseKestrel(ConfigureKestrel);
    });
```

### Content Root

`GetContentRoot(envSetup)` resolves the content root:
- In demo mode → the application's executing folder.
- In production → `EnvironmentSetup.ContentRoot` (configurable via `{APP_PREFIX}_APP_CONTENTROOT`); the directory is created if it does not exist.

### Kestrel Configuration (`WebHost.Kestrel.cs`)

Configured via `WebHostSetup` variables including ports, HTTPS certificate, and binding addresses. Kestrel is set up programmatically from the setup properties. See `WebHostSetup` for the full list of configurable variables.

### Services Configuration (`WebHost.Services.cs`)

`ConfigureWebHostServicesWithContext` registers:
- ASP.NET Core health checks
- Problem Details middleware
- HTTP client factory
- MVC controllers with JSON options
- API versioning (`Asp.Versioning.Mvc`)
- OpenAPI / Scalar documentation
- Security headers (`NWebsec`)
- Authentication (API key, or others depending on registered plugins)
- Authorization
- CORS

Plugin contributions are made via `WebHostPluginBase.ConfigureMvc(MvcOptions)` — all registered `WebHostPluginBase` plugins are invoked after core MVC configuration.

### Application Pipeline (`WebHost.Application.cs`)

`ConfigureApplicationWithContext` builds the middleware pipeline, including:
- HTTPS redirection
- Security headers middleware
- Routing
- Authentication + Authorization
- Controller endpoint mapping

### WebHostSetup

`WebHostSetup : Setup` exposes server configuration as `Variable`-backed properties:
- `HttpPort`, `HttpsPort`
- `SslCertificatePath`, `SslCertificatePassword`
- `ContentRoot`
- `AllowedHosts`
- CORS origins

### Plugin Types for Web

| Plugin Base | Purpose |
|---|---|
| `WebHostPluginBase` | Configures `MvcOptions`, adds middleware |
| `HostPluginBase` | Configures `HostBuilderContext` + `IServiceCollection` |

## Consequences

### Positive

- Single method call (`WebHost.CreateBuilder`) sets up an opinionated, secure-by-default ASP.NET Core host.
- Stage-aware configuration (detailed errors, log level) without any application code.
- Plugin system allows extensions like CloudEvents and API key auth to add MVC configuration without modifying the core `WebHost`.
- Content root management handles demo and production modes transparently.

### Negative

- Opinionated defaults (NWebsec, Hellang ProblemDetails, specific API versioning) may conflict with applications that have different requirements.
- The middleware pipeline order is fixed in `ConfigureApplicationWithContext`; customizing the order requires understanding the internals.
- Kestrel configuration is fully driven by `WebHostSetup` variables — applications that need advanced Kestrel configuration (e.g., multiple endpoints with different protocols) must work around the SDK's configuration layer.
