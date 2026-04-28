# ADR-019: Zio for Cross-Platform File System Abstraction

## Status

Accepted

## Date

2026-03-17

## Context

The SDK's `IFileSystemService` must:

1. Work identically on Windows and Linux (including containers).
2. Support multiple file system contexts: `Machine` (system-wide config), `User` (per-user data), and `Local` (application-local data).
3. Be testable without actual disk I/O.
4. Provide `UPath` (unified, OS-agnostic path) to avoid `\\` vs `/` issues.

`System.IO` is platform-specific in path handling and cannot be abstracted without a wrapper. Direct use of `System.IO.Path` produces OS-specific path strings that fail when code runs across platforms.

## Decision

`xSdk` uses **Zio** as the file system abstraction layer.

### Key Zio Types Used

| Type                 | Description                                                          |
|----------------------|----------------------------------------------------------------------|
| `IFileSystem`        | Abstraction over any file system (physical, virtual, sub)            |
| `PhysicalFileSystem` | Wraps `System.IO` with a `UPath` interface                           |
| `SubFileSystem`      | Read-only view scoped to a subdirectory of another `IFileSystem`     |
| `UPath`              | Immutable, normalized cross-platform path (`/` separated everywhere) |

### FileSystemService

`FileSystemService` implements `IFileSystemService`:

```csharp
public interface IFileSystemService
{
    IFileSystemResult Local { get; }
    IFileSystemResult User { get; }
    IFileSystemResult Machine { get; }
    Task<IFileSystemResult> RequestFileSystemAsync(FileSystemContext context, CancellationToken token);
}
```

`RequestFileSystemAsync` creates:
- `App` — a `SubFileSystem` or `PhysicalFileSystem` rooted at the context's app folder.
- `Data` — a `SubFileSystem` rooted at the context's data folder.

`IFileSystemResult` exposes both the `App` and `Data` file systems as `IFileSystem` instances.

### Root Folder Resolution

`RootFolders` record holds computed `UPath` values for:

| Property      | Windows Example                                | Linux Example                    |
|---------------|------------------------------------------------|----------------------------------|
| `Machine`     | `C:\ProgramData\{company}\{app}`               | `/etc/{company}/{app}`           |
| `User`        | `C:\Users\bob\AppData\Roaming\{company}\{app}` | `~/.config/{company}/{app}`      |
| `Local`       | current working directory                      | current working directory        |
| `MachineData` | `C:\ProgramData\{company}\{app}`               | `/var/lib/{company}/{app}`       |
| `UserData`    | `C:\Users\bob\AppData\Local\{company}\{app}`   | `~/.local/share/{company}/{app}` |

The paths are derived from `EnvironmentSetup.AppCompany` and `EnvironmentSetup.AppName`, so different applications coexist without path collisions.

### Container Support

If `/var/run/configs` exists (Kubernetes `ConfigMap` volume mount pattern), `HostConfigurationManager.LoadAppConfiguration` calls `builder.AddKeyPerFile("/var/run/configs", ...)` to load configuration from individual files — a common pattern for container-native configuration.

### Security

Sensitive paths (certificates, encryption keys) are accessed via the `Machine` or `User` file system. The `Data` sub-file-system separates user-modifiable assets from system-managed ones, enforcing a clear boundary.

## Consequences

### Positive

- `UPath` eliminates path separator issues across Windows and Linux.
- `SubFileSystem` scopes access — code given a `SubFileSystem` cannot escape its root, providing a basic sandboxing guarantee.
- Testable — `MemoryFileSystem` (a Zio type) can replace `PhysicalFileSystem` in unit tests.
- Consistent root paths across the SDK ensure all libraries write to predictable, non-conflicting locations.

### Negative

- `Zio` is a lesser-known library; developers not familiar with it face a learning curve.
- The `IFileSystem` API differs from `System.IO` — copying patterns from standard .NET file I/O code requires adaptation.
- `FileSystemService` is partially initialized during the `SlimHost` pre-build phase (used in `LoadAppConfiguration`); at that point `_envSetup` is `null` in the internal constructor, requiring a special `internal FileSystemService()` constructor without variable service injection.
