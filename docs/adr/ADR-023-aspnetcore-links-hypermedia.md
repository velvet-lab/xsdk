# ADR-023: Hypermedia Links Extension for REST APIs

## Status

Accepted

## Date

2026-03-17

## Context

RESTful APIs following the HATEOAS (Hypermedia as the Engine of Application State) principle include links in responses to guide clients to related resources and available actions. This avoids hard-coding URLs in clients and makes APIs self-describing.

Without a standardized approach, every controller or service would generate link collections ad-hoc, leading to inconsistent link structures and duplication.

## Decision

`xSdk.Extensions.AspNetCore.Links` provides a dedicated hypermedia link extension for ASP.NET Core controllers.

### Structure

```
xSdk.Extensions.AspNetCore.Links/
├── src/
│   ├── Extensions/   ← extension methods and link-building utilities
│   └── Plugins/      ← plugin registration
```

### Link Generation

The extension provides extension methods and/or a service to build `Link` objects (a standard hypermedia link representation with `rel`, `href`, `method`, and optional `title` properties) from controller route names or explicit URLs.

Links are attached to API response objects through a common response envelope pattern, so clients receive both the data and the navigation links in a single response.

### Activation

```csharp
builder.EnableLinks();   // (HostBuilderExtensions in Plugins folder)
```

Registers the `LinksPlugin` which configures the link generation services during `WebHostPluginBase.ConfigureServices`.

### Integration

Link generation services are injected into controllers or services via DI, using `IUrlHelper` (or a custom SDK wrapper) to generate absolute or relative URLs from route names and route values.

## Consequences

### Positive

- Consistent hypermedia link structure across all API endpoints.
- Decoupled from individual controller implementations — link generation logic is centralized.
- Opt-in — applications that do not need HATEOAS are unaffected.

### Negative

- HATEOAS adoption in REST APIs is debated — many API consumers ignore links entirely.
- Link generation requires knowledge of route names, which introduces a coupling between the link-building code and the controller routing configuration.
- The implementation details of this library are limited in the currently available source; further investigation may be needed for complete documentation.
