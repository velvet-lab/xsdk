# ADR-016: CloudNative.CloudEvents Integration

## Status

Accepted

## Date

2026-03-17

## Context

Event-driven architectures often use **CloudEvents** — a CNCF specification for describing event data in a common, vendor-neutral format. ASP.NET Core web APIs that receive CloudEvents must deserialize the structured event envelope correctly from HTTP request bodies.

The `CloudNative.CloudEvents.AspNetCore` package provides ASP.NET Core input formatters for CloudEvents, but they must be registered as MVC input formatters before the controller pipeline is built.

## Decision

`xSdk.Extensions.CloudEvents` integrates CloudEvents support via the SDK plugin system.

### Activation

```csharp
builder.EnableCloudEvents();
```

Registers `CloudEventPlugin` (a `WebHostPluginBase`) via `SlimHost.Instance.PluginSystem`.

### CloudEventPlugin

```csharp
internal class CloudEventPlugin : WebHostPluginBase
{
    public void ConfigureMvc(MvcOptions options)
    {
        var formatter = CloudEventFactory.CreateFormatter();
        options.InputFormatters.Insert(0, new CloudEventJsonInputFormatter(formatter));
    }
}
```

`ConfigureMvc` is called during `WebHost.Services` configuration when all `WebHostPluginBase` plugins are invoked. It inserts a `CloudEventJsonInputFormatter` at position 0, giving it highest priority over other input formatters.

`CloudEventFactory.CreateFormatter()` creates a `JsonEventFormatter` with the SDK's standard `System.Text.Json` configuration (camelCase, enum string conversion, etc.).

### Usage in Controllers

After registration, ASP.NET Core controllers can receive CloudEvents directly:

```csharp
[HttpPost("events")]
public IActionResult ReceiveEvent([FromBody] CloudEvent cloudEvent)
{
    // cloudEvent.Type, cloudEvent.Source, cloudEvent.Data<MyPayload>()
    return Ok();
}
```

The input formatter handles both binary-content-mode (`Content-Type: application/json`) and structured-content-mode (`Content-Type: application/cloudevents+json`) requests according to the CloudEvents HTTP binding specification.

### Extensions

`xSdk.Extensions.CloudEvents/Extensions/` folder contains additional extension methods for working with CloudEvent data — for example, `cloudEvent.Data<TPayload>()` type-safe extraction utilities.

## Consequences

### Positive

- CloudEvents support is zero-touch — one line to enable.
- The formatter is inserted at position 0, ensuring it takes priority; other formatters handle non-CloudEvent requests normally.
- Fully aligned with the CNCF CloudEvents v1.0 specification.
- Works with both CloudEvents content modes (binary and structured).

### Negative

- `CloudEventJsonInputFormatter` is inserted at index 0 unconditionally; if another formatter also claims position 0, there could be conflicts.
- The `CloudEventFactory.CreateFormatter()` serializer settings are fixed; applications with non-standard JSON requirements must customize the formatter registration manually.
- Only HTTP receive (input formatter) is covered; sending CloudEvents (output formatter / `HttpClient` integration) is not part of this extension and must be implemented by the application.
