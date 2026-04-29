# Commit Message Instructions

## Language

Always write commit messages in **English**.

## Format: Conventional Commits

Follow the [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/) specification strictly.

### Structure

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Rules

- The **header** (`type(scope): description`) must not exceed **72 characters**
- The **description** is written in **imperative mood**, lowercase, no trailing period
- Separate header from body, and body from footers, each with exactly **one blank line**
- The **body** is written as a **single continuous line** — no line breaks, no hard wrapping, no trailing spaces within the body
- The **body** explains *what* changed and *why*, not *how*

**Wrong** (line breaks inside body):
```
fix: correct null check in middleware

The middleware threw on startup.
Added a null guard before accessing the context.
```

**Wrong** (hard-wrapped body):
```
fix: correct null check in middleware

The middleware threw on startup. Added a null guard before accessing
the context to prevent the NullReferenceException.
```

**Correct** (single line):
```
fix: correct null check in middleware

The middleware threw on startup. Added a null guard before accessing the context to prevent the NullReferenceException.
```


## Types

| Type       | When to use                                                        |
|------------|--------------------------------------------------------------------|
| `feat`     | A new feature                                                      |
| `fix`      | A bug fix                                                          |
| `docs`     | Documentation only changes                                         |
| `style`    | Formatting, missing semicolons — no logic changes                  |
| `refactor` | Code change that is neither a fix nor a feature                    |
| `perf`     | Performance improvement                                            |
| `test`     | Adding or updating tests                                           |
| `build`    | Changes to build system or external dependencies                   |
| `ci`       | Changes to CI/CD configuration                                     |
| `chore`    | Other changes that don't modify src or test files                  |
| `revert`   | Reverts a previous commit                                          |


## Scopes

Scopes are **only used** for changes within the `libs/`, `demos/`, or `docs/` folders.
All other changes (e.g. CI, build config, tooling, root-level files) are written **without a scope**.

When a scope applies, derive it from the **second segment** of the namespace/project name (lowercase):

| Changed area                           | Scope        |
|----------------------------------------|--------------|
| `libs/xSdk.Data.*`                     | `data`       |
| `libs/xSdk.Extensions.*`              | `extensions` |
| `libs/xSdk.Plugin`                    | `plugin`     |
| `libs/xSdk` (no second segment)       | `xsdk`       |
| `demos/**`                             | `demos`      |
| `docs/**`                              | `docs`       |
| anything else (ci, build, tools, …)    | *(no scope)* |

```
feat(data): add soft-delete support to EntityFramework provider
fix(extensions): resolve middleware ordering issue in AspNetCore
feat(plugin): introduce hot-reload capability
chore(demos): update console demo to use new async API
build: upgrade all projects to .NET 10.0.200
ci: add SonarQube analysis step
```


## Breaking Changes

Breaking changes **must** be indicated in two places:

1. Append `!` after the type/scope in the header
2. Add a `BREAKING CHANGE:` footer that describes what broke and how to migrate

```
feat(xsdk)!: rename IDataStore to IRepository

The IDataStore interface has been renamed to IRepository to better
reflect its purpose and align with domain-driven design terminology.

BREAKING CHANGE: IDataStore has been removed. Replace all usages with
IRepository. The method signatures remain unchanged.
```


## Examples

### Simple feature

```
feat(data): add pagination support to FindAsync in NoSql provider
```

### Bug fix with body

```
fix(extensions): prevent null reference in middleware pipeline

The health-check middleware threw a NullReferenceException when the
request context was not yet fully initialized. Add a null guard before
accessing the context properties.
```

### Breaking change

```
feat(xsdk)!: replace sync methods with async-only API

All synchronous data-access methods have been removed in favour of
their async counterparts to enforce a consistent async programming
model throughout the SDK.

BREAKING CHANGE: Removes all synchronous overloads (Get, Save, Delete).
Migrate call sites to GetAsync, SaveAsync, and DeleteAsync respectively.
```

### Build / tooling change (no scope)

```
build: upgrade all projects to .NET 10.0.200

Update global.json and Directory.Build.props to target the latest
.NET 10 patch release. All package references remain unchanged.
```

### Revert

```
revert: feat(plugin): add hot-reload support

Reverts commit abc1234 because the hot-reload implementation caused
instability in production environments under high load.
```


## Grouping Related Changes

- **One logical change per commit** — do not bundle unrelated modifications
- Keep changes that belong together (e.g., implementation + tests + docs for a single feature) in one commit
- Split large changesets into a series of focused commits in logical order:
  1. Refactoring / preparation
  2. Core implementation
  3. Tests
  4. Documentation / changelog
