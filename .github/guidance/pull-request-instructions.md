# Pull Request Description Instructions

## Language

Always write pull request titles and descriptions in **English**.

## Title

The PR title must be **descriptive and self-explanatory** — a reader should immediately understand what the PR is about without having to read the description.

- Write a clear, human-readable sentence or phrase
- Use title case
- Must not exceed **72 characters**

## Description Structure

Use the following sections. Omit sections that do not apply — but never omit **Summary**.

---

### Summary

A brief overview of the commits included in this PR. Summarize what changed and why in 2–4 sentences. List the most relevant commits or group them by topic if there are many.

### Breaking Changes

Omit this section entirely if there are no breaking changes.

List each breaking change with its migration path:

```
- `IDataStore.FindAsync` signature changed — add `CancellationToken` as last parameter
  **Migrate:** `FindAsync(filter)` → `FindAsync(filter, cancellationToken)`
```

### Testing

Describe how the changes can be verified:
- Which test projects cover the change (`[ProjectName].Tests`)
- Any manual steps required (e.g. running a demo project)
- Edge cases that were explicitly tested

### Related Issues / PRs

Link any related GitHub issues or PRs using `Closes #<n>`, `Fixes #<n>`, or `Ref #<n>`.

---

## Examples

### Feature PR

**Title:** `Add Paged Query Support to All Data Providers`

```markdown
## Summary

Adds a `FindPagedAsync` overload to `IDataStore` and all concrete data providers
(EntityFramework, LiteDB, MongoDB). The existing `FindAsync` returns all matching
documents at once, which is unsuitable for large collections. Relevant commits:

- Add `FindPagedAsync<T>` to `IDataStore` and `DataStoreBase`
- Implement offset-based paging in EF Core provider via `.Skip`/`.Take`
- Implement cursor-based paging in LiteDB and MongoDB providers
- Add unit tests for all three providers
- Add paged-query example to the EF Core demo

## Testing

Run `dotnet test libs/xSdk.Data.EntityFramework/tests` and
`dotnet test libs/xSdk.Data.NoSql/tests`. The EF Core demo (`demos/datalayer-entityframework`)
prints a paged result to the console — verify page 1 and 2 return disjoint sets.

## Related Issues / PRs

Closes #42
```

---

### Bug Fix PR

**Title:** `Fix Null Reference in Health-Check Middleware on Startup`

```markdown
## Summary

The health-check middleware threw a `NullReferenceException` on startup when no
`IHealthCheckService` was registered. A null guard and a descriptive configuration
error message have been added. Relevant commits:

- Add null guard in `HealthCheckMiddleware.InvokeAsync`
- Add test for missing-service scenario

## Testing

`dotnet test libs/xSdk.Extensions.AspNetCore/tests` — the new test
`InvokeAsync_WhenHealthCheckServiceMissing_ThrowsInvalidOperationException` must pass.

## Related Issues / PRs

Fixes #87
```

---

### Breaking Change PR

**Title:** `Replace All Synchronous Data-Access Methods with Async-Only API`

```markdown
## Summary

All synchronous data-access overloads (`Find`, `Save`, `Delete`) have been removed
to enforce a consistent async programming model and eliminate accidental blocking calls.
Relevant commits:

- Remove `Find`, `Save`, `Delete` from `IDataStore` and all implementations
- Update all tests to use async variants only
- Migrate all demo projects to async call sites

## Breaking Changes

- `IDataStore.Find` removed — **Migrate:** use `FindAsync`
- `IDataStore.Save` removed — **Migrate:** use `SaveAsync`
- `IDataStore.Delete` removed — **Migrate:** use `DeleteAsync`

## Testing

`dotnet test` at solution root — all existing tests must remain green after migration.
```
