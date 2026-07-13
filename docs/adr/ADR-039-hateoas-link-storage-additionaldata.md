---
title: "ADR-039: HATEOAS Link Storage in Model AdditionalData"
status: "Accepted"
date: "2026-07-13"
authors: "xSdk Team"
tags: ["architecture", "web", "hypermedia", "hateoas"]
supersedes: ""
superseded_by: ""
---

# ADR-039: HATEOAS Link Storage in Model AdditionalData

## Status

Accepted

## Context

HATEOAS (Hypermedia as the Engine of Application State) is a REST constraint where API responses include hypermedia links that guide clients through available actions and state transitions. For example, a `Customer` resource should include links like:

```json
{
  "id": 123,
  "name": "Acme Corp",
  "_links": {
    "self": { "href": "/api/customers/123" },
    "orders": { "href": "/api/customers/123/orders" },
    "update": { "href": "/api/customers/123", "method": "PUT" }
  }
}
```

The xSdk provides link generation via `xSdk.Extensions.AspNetCore.Links` ([ADR-023](ADR-023-aspnetcore-links-hypermedia.md)), using a policy-driven approach to analyze controller methods and generate links automatically.

**Problem:** Where should generated links be stored in the response model?

**Options considered:**

1. **Dedicated `_links` property** — add `public Dictionary<string, Link> _links { get; set; }` to every model class.
2. **Base class with Links** — require all models to inherit from `HypermediaModel` with a `Links` property.
3. **Separate envelope** — wrap responses in `{ "data": {...}, "_links": {...} }`.
4. **AdditionalData dictionary** — use a flexible `Dictionary<string, object?> AdditionalData` property that models can opt into.

**Requirements:**

- Links must appear in JSON output as `_links` at the model root level.
- Adding links should not require changing model inheritance hierarchy.
- Models without links should not carry empty dictionaries in JSON.
- The solution must work with both `IModel` (DTOs) and `IEntity` (database entities) types.

## Decision

HATEOAS links are stored in `Model.AdditionalData["_links"]` and serialized at the root level via a custom JSON converter.

### Model Contract

`IModel` (from `xSdk.Core`) includes:

```csharp
public interface IModel
{
    Dictionary<string, object?>? AdditionalData { get; set; }
}
```

Base model classes (`EFModel`, `MongoDbModel`, `FlatFileModel`, etc.) implement this property:

```csharp
public abstract class EFModel : IModel
{
    [JsonExtensionData]
    public Dictionary<string, object?>? AdditionalData { get; set; }
}
```

The `[JsonExtensionData]` attribute (System.Text.Json) instructs the serializer to merge `AdditionalData` entries into the root JSON object during serialization.

### Link Storage

`LinksService.AddLinks<TModel>(model, context)` stores generated links in:

```csharp
model.AdditionalData ??= new Dictionary<string, object?>();
model.AdditionalData["_links"] = new Dictionary<string, Link>
{
    { "self", new Link { Href = "/api/customers/123" } },
    { "orders", new Link { Href = "/api/customers/123/orders" } }
};
```

### JSON Serialization

With `[JsonExtensionData]`, the serializer produces:

```json
{
  "id": 123,
  "name": "Acme Corp",
  "_links": {
    "self": { "href": "/api/customers/123" },
    "orders": { "href": "/api/customers/123/orders" }
  }
}
```

Models without links in `AdditionalData` omit the `_links` key entirely.

### Link Generation Pipeline

1. **Controller Action Execution** — returns a model instance.
2. **LinksService.AddLinks** — analyzes the action method, evaluates link policies, and populates `model.AdditionalData["_links"]`.
3. **JSON Serialization** — `[JsonExtensionData]` merges `_links` into the root object.
4. **HTTP Response** — client receives JSON with embedded links.

### Flexibility

`AdditionalData` is a general-purpose extension mechanism. In addition to `_links`, it can store:
- `_meta` — pagination metadata
- `_embedded` — nested resources (HAL-style)
- Custom application data

This allows future extensions without breaking model contracts.

## Consequences

### Positive

- **POS-001**: Links appear at the root level in JSON, conforming to HAL and standard HATEOAS conventions.
- **POS-002**: Models do not need a dedicated `Links` property; inheritance hierarchy remains clean.
- **POS-003**: `[JsonExtensionData]` is a standard System.Text.Json feature; no custom serializer required.
- **POS-004**: Models without links do not carry empty dictionaries in JSON output.
- **POS-005**: `AdditionalData` is extensible for future metadata (pagination, embedded resources, etc.).
- **POS-006**: Works consistently across `IModel` and `IEntity` types (entities can also carry links during projection).

### Negative

- **NEG-001**: `_links` is stored as a dictionary key (`"_links"`); typos in code could cause silent failures.
- **NEG-002**: Developers must remember to check `AdditionalData["_links"]` when inspecting models in code; it's not a strongly-typed property.
- **NEG-003**: `[JsonExtensionData]` merges all `AdditionalData` entries at the root level; potential naming conflicts if another system uses the same key.
- **NEG-004**: Consumers (JavaScript clients, etc.) must handle `_links` as an optional field; strongly-typed clients may need manual deserialization logic.
- **NEG-005**: Link validation (e.g., ensuring `href` is not null) must be done at runtime; no compile-time safety.

## Alternatives Considered

##### Dedicated `_links` Property on Models

- **ALT-001**: **Description**: Add `public Dictionary<string, Link>? _links { get; set; }` to every model class.
- **ALT-002**: **Rejection Reason**: Forces all models to carry a `_links` property, even those that never use HATEOAS. Pollutes model schema.

##### Base Class with Links Property

- **ALT-003**: **Description**: Require all models to inherit from `HypermediaModel : IModel` with a `Links` property.
- **ALT-004**: **Rejection Reason**: Forces inheritance hierarchy change; models that cannot inherit (e.g., records, existing entity classes) are excluded.

##### Separate Envelope Object

- **ALT-005**: **Description**: Wrap responses in `{ "data": {...}, "_links": {...} }`.
- **ALT-006**: **Rejection Reason**: Changes API response shape; breaks existing clients expecting flat model structure. Adds unnecessary nesting.

##### Custom JSON Converter

- **ALT-007**: **Description**: Write a custom `JsonConverter` that injects `_links` at root level without `AdditionalData`.
- **ALT-008**: **Rejection Reason**: Reinvents `[JsonExtensionData]`; adds maintenance burden. The standard attribute works well for this use case.

## Implementation Notes

- **IMP-001**: All base model classes (`EFModel`, `MongoDbModel`, `FlatFileModel`) include `[JsonExtensionData]` on `AdditionalData`.
- **IMP-002**: `LinksService` checks if `AdditionalData` is null and initializes it before adding `_links`.
- **IMP-003**: Link policies (defined via `LinksPolicy`) determine which links are generated; see [ADR-023](ADR-023-aspnetcore-links-hypermedia.md).
- **IMP-004**: If a model already has `AdditionalData["_links"]` (e.g., from manual code), `LinksService` merges or replaces based on configuration.
- **IMP-005**: The `_links` key is a constant defined in `LinksService` to prevent typos: `public const string LinksKey = "_links";`.

## References

- **REF-001**: [ADR-023](ADR-023-aspnetcore-links-hypermedia.md) — ASP.NET Core Links (HATEOAS) Integration
- **REF-002**: [ADR-037](ADR-037-xsdk-data-foundation-layer.md) — xSdk.Data Foundation Layer (IModel definition)
- **REF-003**: [System.Text.Json JsonExtensionData](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts#jsonextensiondataattribute)
- **REF-004**: [HAL Specification](https://stateless.group/hal_specification.html)
- **REF-005**: [HATEOAS (REST)](https://en.wikipedia.org/wiki/HATEOAS)
