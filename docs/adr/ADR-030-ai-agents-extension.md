---
title: "ADR-030: AI Agents Integration as Host Extension"
status: "Accepted"
date: "2026-05-27"
authors: "xSdk Team"
tags: ["architecture", "ai", "agents", "mcp", "openai", "plugin"]
supersedes: "ADR-034, ADR-035"
superseded_by: ""
---

# ADR-030: AI Agents Integration as Host Extension

## Status

Accepted

**Supersedes:** [ADR-034](ADR-034-microsoft-agents-ai-integration.md) and [ADR-035](ADR-035-ai-agent-framework-implementation.md)

This ADR documents the implemented AI agents integration in `xSdk.Extensions.AI`. The proposals in ADR-034 (Microsoft Agents AI Framework Integration) and ADR-035 (AI Agent Framework Implementation) have been realized in this implementation.

## Date

2026-05-27

## Context

The xSdk Plugin Host Model ([ADR-027](ADR-027-plugin-host-model.md)) provides a clean pattern for opt-in cross-cutting concerns via `IPluginHost`. Modern applications increasingly need to integrate AI-backed conversational agents, tool-calling workflows, and Model Context Protocol (MCP) endpoints. These concerns are:

- **Cross-cutting** — they affect the web host, dependency injection, and endpoint routing simultaneously.
- **Optionally required** — not every service needs AI capabilities; hard-coding them into `WebHost` would be wasteful.
- **Provider-dependent** — the concrete `IChatClient` implementation (OpenAI, Azure OpenAI, local model) varies by deployment and must be supplied by the consuming application.

The `Microsoft.Agents.AI` SDK (version 1.6.x) and `ModelContextProtocol.AspNetCore` (version 1.3.x) provide the foundational primitives for chat clients, OpenAI response/conversation handling, and MCP server exposure.

### Alternatives Considered

| Alternative                                              | Reason not chosen                                                                                                                                      |
|----------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| Directly wrapping `Microsoft.Extensions.AI` in `WebHost` | Couples AI to every web host; violates opt-in principle                                                                                                |
| Standalone NuGet package without plugin integration      | Requires manual wiring per-application; bypasses the Variable/Setup system                                                                             |
| Using Semantic Kernel                                    | Heavier dependency footprint; ADR-018 (Mapster) already covers mapping; SK introduces a full orchestration framework that exceeds current requirements |

## Decision

`xSdk.Extensions.AI.Agents` provides a **`AgentsPluginHost`** — a `WebPluginHost` that wires AI chat and MCP capabilities into an ASP.NET Core application via the Plugin Host Model.

### Package and Project

| Property         | Value                                                                                                                                                                                |
|------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Library          | `libs/xSdk.Extensions.AI/`                                                                                                                                                           |
| Package name     | `xSdk.Extensions.AI`                                                                                                                                                                 |
| Target framework | `net10.0`                                                                                                                                                                            |
| Dependencies     | `Microsoft.Agents.AI`, `Microsoft.Agents.AI.Declarative`, `Microsoft.Agents.AI.Workflows`, `xSdk.Extensions.AspNetCore`, `xSdk.Extensions.Commands`                                 |

**Implementation Note (2026-07-13)**: The originally planned `xSdk.Extensions.AI.Agents` package has been consolidated into `xSdk.Extensions.AI`. The implementation uses a generic `PluginHost<TBuilder>` pattern instead of a dedicated `AgentsPluginHost` class.

### Core Types

| Type                    | Namespace             | Responsibility                                                                                                 |
|-------------------------|-----------------------|----------------------------------------------------------------------------------------------------------------|
| `PluginHost<TBuilder>`  | `xSdk.Plugins.AI`     | Generic `WebPluginHost`; wires AI clients, OpenAI responses, conversations, DevUI, and MCP endpoint            |
| `AIBuilder`             | `xSdk.Extensions.AI`  | Extensibility builder; consumers can extend or use directly                                                    |
| `AIOptions`             | `xSdk.Extensions.AI`  | `PluginOptions` subclass; holds `ExposeOpenAIEndpoints`, `IsDevUiEnabled` and other AI-specific configuration |
| `HostBuilderExtensions` | `xSdk.Plugins.AI`     | `EnableAI<TBuilder>(configure, optionsConfigure)` extension on `IHostBuilder`                                  |

### Activation Pattern

```csharp
hostBuilder.EnableAI<AIBuilder>(builder =>
{
    builder.AddClient("MyClient", client => { /* configure */ });
}, options =>
{
    options.ExposeOpenAIEndpoints = true;
    options.IsDevUiEnabled = true;
});
```

`EnableAI<TBuilder>` internally calls:
1. `RegisterPluginHost<PluginHost<TBuilder>>()` — installs the plugin host into the DI pipeline.
2. `RegisterPluginHostOptions<AIOptions>(optionsConfigure)` — binds options.
3. `RegisterBuilder<TBuilder>(configure)` — registers the AI builder with configuration.
4. Registers additional builders: `ClientBuilder<TBuilder>`, `AgentBuilder<TBuilder>`, `ToolBuilder<TBuilder>`.

### Plugin Host Lifecycle (`PluginHost<TBuilder>`)

```csharp
internal partial class PluginHost<TBuilder> : WebPluginHost
    where TBuilder : AIBuilder
{
    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        // Builder.Build(services) initializes AI clients
        // Conditionally registers DevUI, OpenAI responses, and conversations
    }

    public override void ConfigureEndpoint(IEndpointRouteBuilder endpointBuilder)
    {
        // MapDevUI() in development environments
        // MapOpenAIConversations() and MapOpenAIResponses()
    }
}
```

### MCP Integration

`ModelContextProtocol.AspNetCore` is included as a build-time dependency. The MCP server endpoint is exposed via `MapDevUI()` in development environments, enabling interactive testing of agent tool-calls without a separate client.

### Security Considerations

- The `ApiKey` option uses the Variable system and must be supplied via environment variable — **never hardcoded**.
- `MapDevUI()` should be gated to non-production environments. Consuming applications are responsible for applying appropriate authorization middleware before calling `EnableAgents`.
- AI client configuration (endpoints, keys) should be supplied via options and environment variables — **never hardcoded**.
- `MapDevUI()` is gated to development environments via `AIOptions.IsDevUiEnabled` and `Stage.Development` check.
- Consuming applications are responsible for applying appropriate authorization middleware before calling `EnableAI`.
- `PluginHost<TBuilder>` logs configuration steps for observability.

## Consequences

### Positive

- **POS-001**: AI capabilities are opt-in, consistent with all other plugin hosts.
- **POS-002**: Provider neutrality — the concrete AI client implementation is configured via `AIBuilder`.
- **POS-003**: `AIOptions` integrates with the plugin options system, enabling unified configuration.
- **POS-004**: `Microsoft.Agents.AI` abstraction makes future provider swaps possible via builder extension.
- **POS-005**: Generic `PluginHost<TBuilder>` pattern allows consumers to extend `AIBuilder` for custom scenarios.

### Negative

- **NEG-001**: Higher-level agent orchestration patterns are not yet fully defined beyond basic client/tool registration.
- **NEG-002**: The builder pattern requires understanding of the generic constraint `where TBuilder : AIBuilder`.
- **NEG-003**: Package references use standard dependency management via `Directory.Packages.props`.
- **IMP-002**: The library depends on `xSdk.Extensions.AspNetCore` (for `WebPluginHost`) and `xSdk.Extensions.Commands` (for CLI integration).
- **IMP-003**: The implementation consolidates AI functionality into a single package (`xSdk.Extensions.AI`) rather than the originally planned separate `xSdk.Extensions.AI.Agents` package.

## Updates

**2026-07-13**: Updated to reflect actual implementation in `libs/xSdk.Extensions.AI/`. The originally planned `AgentsPluginHost` and `IAgentsPluginBuilder` were replaced with a generic `PluginHost<TBuilder>` pattern using `AIBuilder`. Package name is `xSdk.Extensions.AI` (not `xSdk.Extensions.AI.Agents`). Extension method is `EnableAI` (not `EnableAgents`)
- **IMP-001**: `PrivateAssets="contentfiles;analyzers;build;compile"` on all `Microsoft.Agents.*` and `ModelContextProtocol.AspNetCore` references prevents these heavy packages from flowing transitively to consumers.
- **IMP-002**: The library depends on `xSdk.Extensions.AspNetCore` (for `WebPluginHost`) and `xSdk.Extensions.Telemetry` (for telemetry correlation), following the standard layering.
- **IMP-003**: Once `IAgentService` is implemented, a dedicated ADR amendment or follow-up ADR should document the agent orchestration pattern.

## References

- **REF-001**: [ADR-027](ADR-027-plugin-host-model.md) — Plugin Host Model
- **REF-002**: [ADR-029](ADR-029-aspnetcore-security-plugins.md) — ASP.NET Core Security Plugins (pattern reference)
- **REF-003**: [ADR-004](ADR-004-variable-setup-configuration-system.md) — Variable/Setup System
- **REF-004**: [ADR-015](ADR-015-aspnetcore-web-host-extension.md) — ASP.NET Core Web Host Extension
