---
title: "ADR-040: CloudEvents Schema and Naming Conventions"
status: "Accepted"
date: "2026-07-13"
authors: "xSdk Team"
tags: ["architecture", "events", "cloudevents", "integration"]
supersedes: ""
superseded_by: ""
---

# ADR-040: CloudEvents Schema and Naming Conventions

## Status

Accepted

## Context

The xSdk integrates CloudEvents ([ADR-016](ADR-016-cloudevents-integration.md)) to provide a standardized event format for cross-service communication, pub/sub messaging, and event-driven architectures. CloudEvents defines required and optional attributes:

**Required:**
- `id` — unique event identifier
- `source` — URI identifying the event source
- `type` — event type identifier
- `specversion` — CloudEvents specification version (e.g., "1.0")

**Optional:**
- `datacontenttype` — MIME type of the data payload
- `dataschema` — URI of the schema for the data payload
- `subject` — subject of the event in the context of the source
- `time` — timestamp when the event occurred

Without conventions, teams may use inconsistent values for `source`, `type`, and custom extension attributes, leading to:

- **Routing ambiguity** — consumers cannot reliably filter events by type
- **Source confusion** — unclear which service/component emitted the event
- **Schema drift** — no standard way to reference event payload schemas

The xSdk provides `CloudEventFactory` to create events with default attributes, but conventions for `source`, `type`, and schema URIs must be established.

## Decision

xSdk defines naming conventions for CloudEvents attributes and provides factory methods that enforce these conventions.

### Source URI Convention

`source` is a URI identifying the event producer. Format:

```
{BASE_URL}/{component}/{entity}
```

**Examples:**
- `https://api.example.com/orders/order-service`
- `https://api.example.com/customers/customer-service`

`BASE_URL` is configured via options (e.g., `CloudEventOptions.BaseUrl`). Default: `https://localhost`.

`CloudEventFactory.CreateEvent` accepts `scope` and `component` parameters:

```csharp
var cloudEvent = factory.CreateEvent(
    type: "order.created",
    data: orderData,
    scope: "orders",
    component: "order-service"
);
// source = "https://api.example.com/orders/order-service"
```

### Type Convention

`type` is a reverse-DNS or dot-separated string identifying the event type. Format:

```
{domain}.{entity}.{action}
```

**Examples:**
- `com.example.order.created`
- `com.example.customer.updated`
- `com.example.payment.failed`

For internal SDK events, the domain is omitted:

```
{entity}.{action}
```

**Examples:**
- `order.created`
- `customer.updated`

`CloudEventFactory` does not enforce a specific format but encourages this pattern via documentation and examples.

### Schema URI Convention

`dataschema` is a URI pointing to the JSON schema or definition of the event payload. Format:

```
{SCHEMA_BASE_URL}/{domain}/{entity}/{action}/{version}
```

**Examples:**
- `https://schemas.example.com/com.example/order/created/v1`
- `https://schemas.example.com/com.example/customer/updated/v2`

Schema versioning allows payload evolution without breaking consumers.

`CloudEventFactory` accepts an optional `schemaUri` parameter:

```csharp
var cloudEvent = factory.CreateEvent(
    type: "order.created",
    data: orderData,
    schemaUri: "https://schemas.example.com/com.example/order/created/v1"
);
```

If `schemaUri` is omitted, `dataschema` is not set.

### Custom Extension Attributes

CloudEvents allows custom extension attributes (e.g., `traceparent`, `correlationid`). The xSdk reserves the following extensions:

| Attribute       | Type   | Description                                 | Example                                |
|-----------------|--------|---------------------------------------------|----------------------------------------|
| `traceparent`   | string | W3C Trace Context header                    | `00-abc123-def456-01`                  |
| `correlationid` | string | Correlation ID for request tracing          | `uuid-1234-5678`                       |
| `tenantid`      | string | Multi-tenant identifier (optional)          | `tenant-acme`                          |
| `priority`      | int    | Event priority (1=low, 5=high, 10=critical) | `5`                                    |

Extension attributes are added via `CloudEventFactory.AddExtension(key, value)`:

```csharp
cloudEvent.AddExtension("correlationid", Guid.NewGuid().ToString());
```

### Factory Defaults

`CloudEventFactory` provides default values:

- `specversion` = `"1.0"`
- `datacontenttype` = `"application/json"`
- `time` = `DateTimeOffset.UtcNow`
- `id` = `Guid.NewGuid().ToString()`

These can be overridden per event.

### Serialization Format

CloudEvents are serialized using `CloudNative.CloudEvents.SystemTextJson` formatter. JSON structure:

```json
{
  "specversion": "1.0",
  "id": "abc123-def456",
  "source": "https://api.example.com/orders/order-service",
  "type": "order.created",
  "datacontenttype": "application/json",
  "dataschema": "https://schemas.example.com/com.example/order/created/v1",
  "time": "2026-07-13T10:15:30Z",
  "data": {
    "orderId": 123,
    "customerId": 456,
    "total": 99.99
  },
  "traceparent": "00-abc123-def456-01",
  "correlationid": "uuid-1234-5678"
}
```

## Consequences

### Positive

- **POS-001**: Consistent `source` URIs enable consumers to filter events by component or service.
- **POS-002**: Dot-separated `type` convention aligns with industry standards (e.g., AWS EventBridge, Azure Event Grid).
- **POS-003**: Schema URIs with versioning allow payload evolution without breaking consumers.
- **POS-004**: Reserved extension attributes (`traceparent`, `correlationid`) integrate with distributed tracing and correlation patterns.
- **POS-005**: `CloudEventFactory` provides sensible defaults, reducing boilerplate for common use cases.
- **POS-006**: JSON serialization via `CloudNative.CloudEvents` ensures spec compliance.

### Negative

- **NEG-001**: `source` URI format is a convention, not enforced by factory; developers can pass inconsistent values.
- **NEG-002**: `type` format is not validated; typos or inconsistent naming can occur.
- **NEG-003**: Schema URI registry (e.g., schema server) must be maintained separately; the xSdk does not provide schema hosting.
- **NEG-004**: Extension attributes are untyped; adding custom attributes requires manual key/value pairs with no compile-time safety.
- **NEG-005**: Multi-tenant scenarios must explicitly add `tenantid`; not included by default.

## Alternatives Considered

##### URI Templates for Source

- **ALT-001**: **Description**: Use URI templates (RFC 6570) for `source` format, e.g., `{baseUrl}/{scope}/{component}`.
- **ALT-002**: **Rejection Reason**: Adds dependency on URI template parser; simple string concatenation is sufficient for current requirements.

##### Enum-Based Type Registry

- **ALT-003**: **Description**: Define `EventType` enum with all possible event types; enforce via factory.
- **ALT-004**: **Rejection Reason**: Limits extensibility; consumers cannot define custom event types without modifying the xSdk.

##### Embedded Schema in Event

- **ALT-005**: **Description**: Embed JSON schema directly in `dataschema` as an object instead of a URI.
- **ALT-006**: **Rejection Reason**: Inflates event payload size; CloudEvents spec expects `dataschema` to be a URI, not an embedded schema.

##### Mandatory Correlation ID

- **ALT-007**: **Description**: Require `correlationid` on all events; enforce via factory.
- **ALT-008**: **Rejection Reason**: Not all events are part of a correlation flow (e.g., telemetry, logs); making it mandatory adds unnecessary overhead.

## Implementation Notes

- **IMP-001**: `CloudEventFactory` is registered as a singleton; `BaseUrl` and `SchemaBaseUrl` are injected via `CloudEventOptions`.
- **IMP-002**: The factory uses `CloudEventFormatter` from `CloudNative.CloudEvents.SystemTextJson` for JSON serialization.
- **IMP-003**: Extension attributes are added via the CloudEvents SDK's `SetAttributeFromString` method; no custom serializer required.
- **IMP-004**: Schema URI format is documented in `CloudEventFactory` XML comments; consumers are encouraged to host schemas in a versioned registry (e.g., Azure Schema Registry, AWS Glue).
- **IMP-005**: `source` and `type` conventions are documented in README and wiki; validation tooling (e.g., analyzer, linter) is planned for future releases.

## References

- **REF-001**: [ADR-016](ADR-016-cloudevents-integration.md) — CloudEvents Integration
- **REF-002**: [CloudEvents Specification v1.0](https://github.com/cloudevents/spec/blob/v1.0/spec.md)
- **REF-003**: [CloudNative.CloudEvents NuGet Package](https://www.nuget.org/packages/CloudNative.CloudEvents)
- **REF-004**: [W3C Trace Context](https://www.w3.org/TR/trace-context/)
- **REF-005**: [JSON Schema Registry Patterns](https://www.confluent.io/blog/schema-registry-kafka-stream-processing-yes-virginia-you-really-need-one/)
