# Pull Request Description Instructions

## Language

Always write pull request titles and descriptions in **English**.

## Title

The PR title **must** follow the same [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/)
format used for commit messages. It becomes the squash-merge commit and feeds into semantic-release.

```
<type>[optional scope]: <description>
```

- Use the same **types** and **scope** rules defined in the commit instructions
- Use **imperative mood**, lowercase, no trailing period
- Must not exceed **72 characters**
- Append `!` and add a `BREAKING CHANGE:` section in the body for breaking changes

## Description Structure

Use the following sections. Omit sections that do not apply — but never omit **Summary**.

---

### Summary

A short paragraph (2–4 sentences) explaining what this PR changes and **why**.
Describe the problem or motivation first, then the solution.

### Changes

A bullet list grouping related changes. Group by area (e.g. library, demo, CI).
Keep each bullet concise — one line per logical change.

```
- `libs/xSdk.Data`: add `FindPagedAsync` overload to `IDataStore`
- `libs/xSdk.Data.EntityFramework`: implement `FindPagedAsync` using `.Skip`/`.Take`
- `libs/xSdk.Data.NoSql`: implement `FindPagedAsync` using cursor-based paging
- `demos/datalayer-entityframework`: demonstrate paged query in `Program.cs`
```

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

## Scope Rules (same as commit messages)

| Changed area              | Scope        |
|---------------------------|--------------|
| `libs/xSdk.Data.*`        | `data`       |
| `libs/xSdk.Extensions.*`  | `extensions` |
| `libs/xSdk.Plugin`        | `plugin`     |
| `libs/xSdk` (core)        | `xsdk`       |
| `demos/**`                | `demos`      |
| `docs/**`                 | `docs`       |
| CI, build, tooling, root  | *(no scope)* |

## Examples

### Feature PR

**Title:** `feat(data): add paged query support to IDataStore`

```markdown
## Summary

The current `FindAsync` API returns all matching documents at once, which is
unsuitable for large collections. This PR introduces a `FindPagedAsync` overload
to all data providers to support cursor- and offset-based paging.

## Changes

- `libs/xSdk.Data`: add `FindPagedAsync<T>` to `IDataStore` and `DataStoreBase`
- `libs/xSdk.Data.EntityFramework`: implement via `.Skip`/`.Take` with total-count projection
- `libs/xSdk.Data.NoSql`: implement via LiteDB cursor paging
- `libs/xSdk.Data.MongoDB`: implement via MongoDB `IAsyncCursor`
- `libs/xSdk.Data.*`: unit tests for all three providers
- `demos/datalayer-entityframework`: add paged-query example to `Program.cs`

## Testing

Run `dotnet test libs/xSdk.Data.EntityFramework/tests` and
`dotnet test libs/xSdk.Data.NoSql/tests`. The EF Core demo (`demos/datalayer-entityframework`)
prints a paged result to the console — verify page 1 and 2 return disjoint sets.

## Related Issues / PRs

Closes #42
```

---

### Bug Fix PR

**Title:** `fix(extensions): resolve null reference in health-check middleware`

```markdown
## Summary

The health-check middleware threw a `NullReferenceException` on startup when no
`IHealthCheckService` was registered. A null guard and a clear configuration error
message have been added.

## Changes

- `libs/xSdk.Extensions.AspNetCore`: add null guard in `HealthCheckMiddleware.InvokeAsync`
- `libs/xSdk.Extensions.AspNetCore.Tests`: add test for missing-service scenario

## Testing

`dotnet test libs/xSdk.Extensions.AspNetCore/tests` — the new test
`InvokeAsync_WhenHealthCheckServiceMissing_ThrowsInvalidOperationException` must pass.

## Related Issues / PRs

Fixes #87
```

---

### Breaking Change PR

**Title:** `feat(xsdk)!: replace sync data-access methods with async-only API`

```markdown
## Summary

All synchronous data-access overloads have been removed to enforce a consistent
async programming model. This eliminates accidental blocking calls and aligns the
SDK with .NET best practices.

## Changes

- `libs/xSdk.Data`: remove `Find`, `Save`, `Delete` from `IDataStore`
- `libs/xSdk.Data.*`: remove all synchronous implementations
- `libs/xSdk.Data.*`: update tests to use async variants only
- `demos/**`: migrate all demo projects to async call sites

## Breaking Changes

- `IDataStore.Find` removed — **Migrate:** use `FindAsync`
- `IDataStore.Save` removed — **Migrate:** use `SaveAsync`
- `IDataStore.Delete` removed — **Migrate:** use `DeleteAsync`

## Testing

`dotnet test` at solution root — all existing tests must remain green after migration.
```
