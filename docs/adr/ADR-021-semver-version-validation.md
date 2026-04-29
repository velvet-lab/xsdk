# ADR-021: Semantic Versioning for Plugin Compatibility

## Status

Accepted

## Date

2026-03-17

## Context

The SDK uses a dynamic plugin system (see [ADR-003](ADR-003-plugin-extensibility-model.md)) where plugin assemblies can be loaded at runtime from the file system. When a plugin is loaded, the host must verify that the plugin's declared SDK version requirement is compatible with the running SDK version, to prevent:

- Loading a plugin built against a newer API that the current host does not have.
- Loading an outdated plugin that uses deprecated or removed APIs.
- Allowing patch-level updates without forcing re-verification.

## Decision

`SemVer` is a value object in `xSdk.Plugin` that wraps the **SemanticVersioning** library (`SemanticVersioning.Version` and `SemanticVersioning.Range`).

### SemVer

```csharp
public sealed class SemVer
{
    public SemVer(string version) { ... }          // e.g., "1.2.3" or "~1.2.3" or ">=1.0.0 <2.0.0"
    public SemVer(string version, string range) { ... }

    public bool Satisfies(string version) { ... }  // checks if version satisfies this object's range
    public bool IsCompatible(SemVer other) { ... } // bidirectional compatibility check
}
```

Range notation follows the npm semver convention:
- `~1.2.3` — compatible with `1.2.x` (patch-level tolerance)
- `^1.2.3` — compatible with `1.x.y` (minor-level tolerance)
- `>=1.0.0 <2.0.0` — explicit range

### Host Version Validation

`SlimHostBuilder.ValidateAppVersion(appVersion)` is called during `SlimHostInternal.InitializeSlimHost` after the `EnvironmentSetup.AppVersion` is loaded from configuration. If the version string is non-empty, it is parsed into a `SemVer` and validated against the SDK's built-in minimum version requirement.

### Plugin Compatibility Check

When `PluginService` loads a plugin (via `Weikio.PluginFramework`), the plugin's `PluginDescription` declares a `PluginVersion` (`SemVer`) and a `SdkVersion` requirement. The host compares these:

```csharp
if (!hostSdkVersion.IsCompatible(plugin.SdkVersion))
    throw new SdkException($"Plugin {plugin.Name} requires SDK {plugin.SdkVersion}; current: {hostSdkVersion}");
```

### Version String Normalization

`SemVer` handles non-standard version strings produced by CI pipelines (e.g., `1.2.3-beta.1+build.456`). Range strings with `x` placeholders (`1.2.x`) are normalized to `SemanticVersioning.Range` format.

## Consequences

### Positive

- Incompatible plugins are rejected early (at load time) rather than failing at runtime when an API is called.
- Patch-level updates (`~1.2.0`) are allowed automatically, reducing maintenance overhead.
- Plugin authors can declare precise compatibility ranges, enabling the SDK to evolve without breaking all existing plugins simultaneously.

### Negative

- `SemanticVersioning` library is an additional dependency (though small).
- The npm-style range syntax (`~`, `^`, `>=`) is not part of the .NET versioning tradition — developers may be unfamiliar.
- If `AppVersion` is not set in configuration, version validation is skipped silently — there is no enforcement that applications always declare a version.
