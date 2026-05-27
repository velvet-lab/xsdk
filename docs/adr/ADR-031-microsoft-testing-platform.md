---
title: "ADR-031: Microsoft Testing Platform for .NET Test Execution"
status: "Accepted"
date: "2026-05-27"
authors: "xSdk Team"
tags: ["architecture", "testing", "xunit", "mtp", "ci"]
supersedes: ""
superseded_by: ""
---

# ADR-031: Microsoft Testing Platform for .NET Test Execution

## Status

Accepted

## Date

2026-05-27

## Context

The xSdk repository uses xUnit for unit and integration tests across all libraries. Historically, .NET test execution relied on the legacy `VSTest` runner (invoked via `dotnet test`). As of .NET 8+, Microsoft introduced the **Microsoft Testing Platform (MTP)** — a leaner, first-party test execution engine that replaces VSTest as the default runner for modern .NET test projects.

Key motivations for migrating:

1. **Performance** — MTP starts test processes faster and reduces per-project overhead in large solutions.
2. **Unified runner configuration** — `global.json` centralizes the runner choice; no per-project runner flags are needed.
3. **xUnit v3 compatibility** — xUnit v3 is designed for MTP and exposes a dedicated `xunit.v3.mtp-v2` adapter.
4. **Native coverage support** — `Microsoft.Testing.Extensions.CodeCoverage` integrates directly without requiring a separate Coverlet or OpenCover step.
5. **`dotnet run`-compatible test binaries** — MTP test projects are executable; they can be run with `dotnet run` for single-project debugging.

### Alternatives Considered

| Alternative | Reason not chosen |
|-------------|-------------------|
| Stay on VSTest + xUnit v2 | VSTest is a legacy path; no active feature development; coverage tooling is external |
| NUnit + MTP | xUnit v3 + MTP is the officially preferred combination and already used throughout the repo |
| Coverlet for coverage | `Microsoft.Testing.Extensions.CodeCoverage` is the first-party solution; eliminates a tool dependency |

## Decision

All test projects in xSdk are configured to use the **Microsoft Testing Platform** with **xUnit v3**.

### Global Configuration (`global.json`)

```json
{
  "sdk": {
    "version": "10.0.300",
    "rollForward": "latestFeature"
  },
  "test": {
    "runner": "Microsoft.Testing.Platform"
  }
}
```

The `"test.runner": "Microsoft.Testing.Platform"` entry causes `dotnet test` to delegate to MTP instead of VSTest for the entire solution.

### Project-Level Configuration (`Directory.Build.props`)

All test projects (identified by `<IsTestProject>true</IsTestProject>`) receive the following shared settings:

```xml
<PropertyGroup>
  <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
  <TestingPlatformCaptureOutput>false</TestingPlatformCaptureOutput>
  <TestingPlatformCommandLineArguments>
    $(TestingPlatformCommandLineArguments) --config-file $(SolutionDir)testconfig.json
  </TestingPlatformCommandLineArguments>
</PropertyGroup>

<ItemGroup Condition="'$(IsTestProject)' == 'true'">
  <Using Include="Xunit" />
  <PackageReference Include="Microsoft.Extensions.Hosting.Testing" />
  <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" />
  <PackageReference Include="xunit.v3.mtp-v2" />
</ItemGroup>
```

| Property / Package | Purpose |
|--------------------|---------|
| `TestingPlatformDotnetTestSupport=true` | Enables MTP mode for `dotnet test` |
| `TestingPlatformCaptureOutput=false` | Allows test output to flow directly to the terminal |
| `--config-file testconfig.json` | Points all test processes at the shared test configuration file |
| `xunit.v3.mtp-v2` | xUnit v3 MTP adapter; replaces the legacy `xunit.runner.visualstudio` |
| `Microsoft.Testing.Extensions.CodeCoverage` | First-party code coverage collection (replaces Coverlet) |
| `Microsoft.Extensions.Hosting.Testing` | Microsoft's integration testing host utilities used for plugin/host fixtures |

### Test Configuration (`testconfig.json`)

A shared `testconfig.json` in the solution root centralizes test settings (e.g., parallelism, retry policy, output verbosity) that apply to all test runners. This prevents per-project duplication and ensures consistent CI behavior.

### Integration Testing Pattern

Integration tests that involve the plugin pipeline use `Microsoft.Extensions.Hosting.Testing` fixtures following the convention established in the codebase:

```csharp
// Non-web plugins
public class MyTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>

// Web plugins (authentication, web security, etc.)
public class MyWebTests(WebTestHostFixture fixture) : IClassFixture<WebTestHostFixture>
```

`TestHostFixture` and `WebTestHostFixture` are resolved via the MTP-aware DI test host, replacing raw `new ServiceCollection()` usage.

### CI Integration

On GitHub Actions, test execution runs as:

```bash
dotnet test --no-build --configuration Release
```

MTP is selected automatically via `global.json`. Coverage output is collected in the standard `TestResults/` folder for subsequent SonarQube or LCOV upload.

## Consequences

### Positive

- **POS-001**: Faster test execution — MTP eliminates the VSTest process-launcher overhead.
- **POS-002**: Single runner configuration in `global.json`; no per-project or per-workflow `--runner` flags needed.
- **POS-003**: First-party coverage collection without Coverlet/OpenCover toolchain complexity.
- **POS-004**: `testconfig.json` enforces consistent parallel execution and verbosity across all projects and CI runs.
- **POS-005**: xUnit v3 APIs (e.g., `Assert.Multiple`, improved async support) are available to all test projects.

### Negative

- **NEG-001**: `Microsoft.Extensions.Hosting.Testing` is a preview package (`10.4.0-preview.1.26160.2`) — API stability is not guaranteed until .NET 10 RTM.
- **NEG-002**: Third-party IDE plugins (Rider, older VS versions) may not yet fully support MTP test discovery; VSTest-based discovery requires the legacy adapter package.
- **NEG-003**: The `xunit.v3.mtp-v2` package name signals a transitional adapter (`mtp-v2`); a rename or consolidation with the stable xUnit v3 release is expected.

## Implementation Notes

- **IMP-001**: The `<Using Include="Xunit" />` global using is added for all test projects — no per-file `using Xunit;` is required.
- **IMP-002**: Coverage results are written to `TestResults/coverage.xml` (tracked at root level) and `TestResults/` per-project trx files.
- **IMP-003**: `xsdk.runsettings` exists for legacy VSTest compatibility (e.g., Rider) but is not the primary configuration source for CI.

## References

- **REF-001**: [Microsoft Testing Platform documentation](https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-intro)
- **REF-002**: [xUnit v3 documentation](https://xunit.net/docs/getting-started/v3/microsoft-testing-platform)
- **REF-003**: `global.json` — `"test": { "runner": "Microsoft.Testing.Platform" }`
- **REF-004**: `Directory.Build.props` — shared test project settings
