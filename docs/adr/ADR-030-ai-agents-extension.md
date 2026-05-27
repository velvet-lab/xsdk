---
title: "ADR-030: AI Agents Integration as Host Extension"
status: "Accepted"
date: "2026-05-27"
authors: "xSdk Team"
tags: ["architecture", "ai", "agents", "mcp", "openai", "plugin"]
supersedes: ""
superseded_by: ""
---

# ADR-030: AI Agents Integration as Host Extension

## Status

Accepted

## Date

2026-05-27

## Context

The xSdk Plugin Host Model ([ADR-027](ADR-027-plugin-host-model.md)) provides a clean pattern for opt-in cross-cutting concerns via `IPluginHost`. Modern applications increasingly need to integrate AI-backed conversational agents, tool-calling workflows, and Model Context Protocol (MCP) endpoints. These concerns are:

- **Cross-cutting** — they affect the web host, dependency injection, and endpoint routing simultaneously.
- **Optionally required** — not every service needs AI capabilities; hard-coding them into `WebHost` would be wasteful.
- **Provider-dependent** — the concrete `IChatClient` implementation (OpenAI, Azure OpenAI, local model) varies by deployment and must be supplied by the consuming application.

The `Microsoft.Agents.AI` SDK (version 1.6.x) and `ModelContextProtocol.AspNetCore` (version 1.3.x) provide the foundational primitives for chat clients, OpenAI response/conversation handling, and MCP server exposure.

### Alternatives Considered

| Alternative | Reason not chosen |
|-------------|-------------------|
| Directly wrapping `Microsoft.Extensions.AI` in `WebHost` | Couples AI to every web host; violates opt-in principle |
| Standalone NuGet package without plugin integration | Requires manual wiring per-application; bypasses the Variable/Setup system |
| Using Semantic Kernel | Heavier dependency footprint; ADR-018 (Mapster) already covers mapping; SK introduces a full orchestration framework that exceeds current requirements |

## Decision

`xSdk.Extensions.AI.Agents` provides a **`AgentsPluginHost`** — a `WebPluginHost` that wires AI chat and MCP capabilities into an ASP.NET Core application via the Plugin Host Model.

### Package and Project

| Property | Value |
|----------|-------|
| Library  | `libs/xSdk.Extensions.Agents/` |
| Package name | `xSdk.Extensions.AI.Agents` |
| Target framework | `net10.0` |
| Dependencies | `Microsoft.Agents.AI`, `Microsoft.Agents.AI.DevUI`, `Microsoft.Agents.AI.Hosting.OpenAI`, `Microsoft.Agents.AI.OpenAI`, `ModelContextProtocol.AspNetCore`, `Microsoft.AspNetCore.OpenApi` |

### Core Types

| Type | Namespace | Responsibility |
|------|-----------|----------------|
| `AgentsPluginHost` | `xSdk.Plugins.AI.Agents` | `WebPluginHost`; wires `IChatClient`, OpenAI responses, conversations, DevUI, and MCP endpoint |
| `IAgentsPluginBuilder` | `xSdk.Extensions.AI.Agents` | Extensibility interface; consuming code implements `CreateChatClient()` |
| `DefaultAgentsPluginBuilder` | `xSdk.Plugins.AI.Agents` | Internal default; throws `NotImplementedException` — forces consumers to supply a real builder |
| `AgentsPluginOptions` | `xSdk.Extensions.AI.Agents` | `PluginOptions` subclass; holds `Endpoint` and `ApiKey` via the Variable system ([ADR-004](ADR-004-variable-setup-configuration-system.md)) |
| `IAgentService` | `xSdk.Extensions.AI.Agents` | Placeholder interface for higher-level agent orchestration (not yet implemented) |
| `HostBuilderExtensions` | `xSdk.Plugins.AI.Agents` | `EnableAgents<TPluginBuilder>(configureOptions)` extension on `IHostBuilder` |

### Activation Pattern

```csharp
hostBuilder.EnableAgents<MyOpenAiPluginBuilder>(options =>
{
    options.Endpoint = "https://api.openai.com/v1";
    // ApiKey is read from environment via Variable system
});
```

`EnableAgents<TPluginBuilder>` internally calls:
1. `RegisterPluginHost<AgentsPluginHost>()` — installs the plugin host into the DI pipeline.
2. `RegisterPluginHostOptions<AgentsPluginOptions>(configureOptions)` — binds options via Variable system.
3. `RegisterPluginBuilder<IAgentsPluginBuilder, TPluginBuilder>()` — registers the consumer's concrete builder.

### Plugin Host Lifecycle (`AgentsPluginHost`)

```csharp
internal class AgentsPluginHost(...) : WebPluginHost
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Resolves IAgentsPluginBuilder and calls CreateChatClient()
        // Registers IChatClient, OpenAI responses, conversations, DevUI
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        // No middleware needed at this time
    }

    public override void ConfigureEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDevUI(); // DevUI endpoint for development
    }
}
```

### MCP Integration

`ModelContextProtocol.AspNetCore` is included as a build-time dependency. The MCP server endpoint is exposed via `MapDevUI()` in development environments, enabling interactive testing of agent tool-calls without a separate client.

### Security Considerations

- The `ApiKey` option uses the Variable system and must be supplied via environment variable — **never hardcoded**.
- `MapDevUI()` should be gated to non-production environments. Consuming applications are responsible for applying appropriate authorization middleware before calling `EnableAgents`.
- `AgentsPluginHost` logs an error (rather than throwing) when `IAgentsPluginBuilder` cannot be resolved, ensuring startup does not crash but the misconfiguration is visible in logs.

## Consequences

### Positive

- **POS-001**: AI capabilities are opt-in, consistent with all other plugin hosts.
- **POS-002**: Provider neutrality — the concrete `IChatClient` is injected by the consumer; the plugin host has no compile-time dependency on a specific AI provider.
- **POS-003**: `AgentsPluginOptions` integrates with the Variable system, enabling unified CLI/env-var configuration.
- **POS-004**: `Microsoft.Extensions.AI` abstraction (`IChatClient`) makes future provider swaps (e.g., Azure OpenAI → local model) a one-line change in `IAgentsPluginBuilder.CreateChatClient()`.

### Negative

- **NEG-001**: `IAgentService` interface is currently a stub — higher-level agent orchestration patterns are not yet defined.
- **NEG-002**: `DefaultAgentsPluginBuilder.CreateChatClient()` throws `NotImplementedException`; consumers who call `EnableAgents` without providing a real builder will fail at runtime (not at compile time).
- **NEG-003**: MCP and DevUI endpoints are included unconditionally at build time; environment-gating is the consumer's responsibility.

## Implementation Notes

- **IMP-001**: `PrivateAssets="contentfiles;analyzers;build;compile"` on all `Microsoft.Agents.*` and `ModelContextProtocol.AspNetCore` references prevents these heavy packages from flowing transitively to consumers.
- **IMP-002**: The library depends on `xSdk.Extensions.AspNetCore` (for `WebPluginHost`) and `xSdk.Extensions.Telemetry` (for telemetry correlation), following the standard layering.
- **IMP-003**: Once `IAgentService` is implemented, a dedicated ADR amendment or follow-up ADR should document the agent orchestration pattern.

## References

- **REF-001**: [ADR-027](ADR-027-plugin-host-model.md) — Plugin Host Model
- **REF-002**: [ADR-029](ADR-029-aspnetcore-security-plugins.md) — ASP.NET Core Security Plugins (pattern reference)
- **REF-003**: [ADR-004](ADR-004-variable-setup-configuration-system.md) — Variable/Setup System
- **REF-004**: [ADR-015](ADR-015-aspnetcore-web-host-extension.md) — ASP.NET Core Web Host Extension
