# ADR-004: Variable/Setup System for Configuration

## Status

Accepted

## Date

2026-03-17, updated 2026-05-31

## Context

.NET configuration (`IConfiguration`) is flat key-value and requires manual binding. The SDK needs a richer configuration model that:

1. Supports multiple sources in priority order: environment variables, command-line arguments, YAML/JSON files, container secrets (`/var/run/configs`).
2. Allows configuration values to be grouped into typed, validated "Setup" objects (similar to `IOptions<T>` but pre-DI-ready).
3. Uses application-specific prefixes (e.g., `APP_PREFIX=de`) so environment variables do not clash between applications.
4. Supports read/write at runtime (e.g., `Stage`, `IsDemo` can be toggled programmatically).
5. Supports "hidden" and "protected" variables (secrets that should not appear in help text or diagnostics).
6. Allows variables to be exposed as CLI flags automatically (via `CommandLine` key derivation).
7. Supports resource aliases (e.g., `app.name`, `{{app.prefix}}.environment.stage`).

## Decision

A two-layer system is introduced: **`Variable`** (metadata) and **`Setup`** (typed access).

### Variable

`Variable` / `Variable<TType>` represents a single configuration entry with:

| Property      | Description                                                                                                     |
|---------------|-----------------------------------------------------------------------------------------------------------------|
| `Name`        | Qualified logical name. When `[VariablePrefix]` is set on the options class, the prefix is automatically prepended at read time: e.g. `"file-path"` with prefix `"flat-file"` yields `"flat-file-file-path"`. Raw name is stored internally. |
| `ValueType`   | CLR type for the value                                                                                          |
| `Prefix`      | Class-level prefix (set via `[VariablePrefix("...")]`). Only present when the attribute is explicitly declared — no auto-derivation from class name. |
| `NoPrefix`    | Per-variable override: skip the class prefix for this specific variable                                         |
| `Template`    | CLI template stored as raw input (e.g. `"--path <path>"`). Rendered with the prefix prepended at read time: e.g. `"--flat-file-path <path>"`. |
| `IsProtected` | Marks as secret — value never logged                                                                            |
| `IsHidden`    | Excluded from help/diagnostic output                                                                            |
| `HelpText`    | Description for CLI help                                                                                        |

Keys are derived deterministically:
- **Environment / file system** key: `{APP_PREFIX}_{VARIABLE_NAME}` (upper-case)
- **Command-line** key: `--{app-prefix}-{variable-name}` (lower-case, hyphenated)
- **File key**: same as system key with `_FILE` suffix (for Docker secrets pattern)

### Setup

`Setup` is an abstract base class. Concrete setups declare their variables as properties decorated with `[Variable(...)]`. Values are read/written through `ReadValue<TValue>(name)` / `SetValue(name, value)`, which delegate to the underlying `IVariableService`. The raw short name (e.g. `"file-path"`) is used in the property body — prefix qualification is resolved automatically by the service.

```csharp
[VariablePrefix("flat-file")]
public class FlatFileDatabaseOptions : DatabaseOptions
{
    [Variable(name: "file-path", template: "--path <path>", helpText: "...")]
    public string FilePath
    {
        get => ReadValue<string>("file-path");   // resolves to "flat-file-file-path" internally
        set => SetValue("file-path", value);
    }
}
```

A class **without** `[VariablePrefix]` has no prefix — the auto-derivation from the class name was removed. The former `[VariableNoPrefix]` attribute is therefore no longer needed and has been deleted.

### IVariableService

`VariableService` (internal) manages the variable registry and resolves values from, in order of priority:

1. Environment variables (loaded in `AddEnvironmentVariables()`)
2. `IConfiguration` (app settings files, key-per-file, etc.)
3. Registered default values from the `[Variable]` attribute

Setups are registered via:
```csharp
builder.RegisterSetup<TSetup>();
builder.RegisterSetup<TSetup>(configure => { ... });
```

`IVariableService.GetSetup<TSetup>()` returns the initialized, validated setup instance.

### Providers

`VariableService` uses a provider chain (implementations in `Extensions/Variable/Providers/`) that maps variable names to their resolved values across the different configuration sources.

### Validation

`Setup.Validate()` runs:
1. Data Annotation validation on all properties.
2. Virtual `ValidateSetup()` for custom business rules.

Validation can throw `SdkException` or collect `ValidationResult` objects.

## Consequences

### Positive

- Strong typing for all configuration values — no string-key lookups in application code.
- Unified source of truth for variable metadata (help text, defaults, prefix rules).
- Prefix is opt-in via `[VariablePrefix("...")]`: no surprise namespace derived from the class name.
- `Variable.Name` and `Variable.Template` are computed at read time from the stored prefix — no duplication, no drift.
- Collision detection: `VariableService` logs a warning when two registrations produce the same qualified name.
- Automatic CLI flag derivation means configuration properties are exposed as command-line arguments without extra code.
- Protected/hidden flags prevent secrets from leaking in logs or help output.
- Resource aliases enable tenant-specific naming conventions.

### Negative

- Another configuration abstraction on top of `IConfiguration` — developers must learn the `Setup`/`Variable` API in addition to standard .NET configuration.
- The `ReadValue`/`SetValue` indirection through string names loses compile-time name safety (mitigated by the `[Variable(name: ...)]` attribute enforcing a contract).
- The `SlimHost.Instance` dependency inside `Variable` means variables cannot be created before the host is initialized.
