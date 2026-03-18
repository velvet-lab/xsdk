# ADR-017: Spectre.Console.Cli for Command-Line Interface

## Status

Accepted

## Date

2026-03-17

## Context

Many SDK-based applications expose a CLI — either as their primary interface (tools, CLIs) or as a secondary interface for administration and diagnostics. The SDK must provide:

1. A structured, typed command model with sub-commands.
2. Help text generation.
3. Integration with the `Variable`/`Setup` system so CLI flags are derived from the same variable definitions as environment variables.
4. An SDK-friendly way to register commands into the host.

`System.CommandLine` was considered but was still in beta during the design phase. `Spectre.Console.Cli` is a mature, production-ready library with a clean typed command API and excellent help text rendering.

## Decision

`xSdk.Extensions.Commands` uses **Spectre.Console.Cli** for CLI integration.

### Activation

```csharp
builder.EnableCommands();
```

Registers `CommandPlugin` (a `PluginBase`) via the plugin system. `CommandPlugin` itself is a stub — it participates in the plugin pipeline to allow other plugins to contribute commands via `ITelemetryPluginBuilder`-style extension points.

### Command Infrastructure

Commands are defined by subclassing Spectre.Console.Cli's `Command<TSettings>` or `AsyncCommand<TSettings>`. Settings classes map to `Variable` definitions — the SDK's `Variable` system derives CLI key names using `CreateKey(forCommandline: true, withApplicationPrefix: false)`, producing hyphenated lowercase names (e.g., `--environment-stage`, `--enable-demo`).

### Extension Point

`xSdk.Extensions.Commands/Extensions/Commands/` provides `ICommandPluginBuilder` (analogous to `ITelemetryPluginBuilder`) so separate plugins can register their own commands into the `CommandApp` without modifying the core command infrastructure.

### Default Commands

`DefaultCommandSettings` defines the standard SDK variables that are also exposed as CLI flags:

| CLI Flag | Mapped Variable |
|---|---|
| `--environment-stage` | `Stage` |
| `--environment-demo` | `IsDemo` |
| `--log-level` | Log level override |

These are always available in any SDK-based CLI application.

### HostBuilderExtensions

```csharp
builder.EnableCommands();
// application-level:
builder.RegisterSetup<MyAppCommandSettings>();
```

`RegisterSetup` binds the CLI settings class into the `VariableService`, enabling round-tripping: values set via CLI are read by the `Setup` system just like environment variables.

## Consequences

### Positive

- Spectre.Console.Cli provides beautiful help output and tab completion.
- Typed settings classes eliminate string-based argument parsing.
- CLI flags are automatically derived from the `Variable`/`Setup` system — no duplication.
- Plugin extension point allows modular CLI command registration.

### Negative

- Spectre.Console's `CommandApp` has its own DI-integration via `ITypeRegistrar`; bridging it to `Microsoft.Extensions.DependencyInjection` requires a custom registrar adapter.
- The CLI activation model (`EnableCommands()`) adds overhead even for applications that only expose a web API and do not use the CLI.
- `DefaultCommandSettings.Definitions` hard-codes SDK-level CLI flags; application-specific flags must be registered separately.
