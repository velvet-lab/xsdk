# Review Selection Instructions

## Language

Always respond to code reviews in **English**.

## Scope

This review targets the **selected code snippet only**. Focus on what is visible in the
selection. Do not speculate about code outside the selection unless clearly relevant.

## Severity Categories

Categorize every finding with one of these labels:

| Label | When to use |
|---|---|
| 🔴 **CRITICAL** | Security vulnerabilities, logic errors, data corruption, breaking API changes |
| 🟡 **IMPORTANT** | SOLID violations, missing tests for new logic, visible performance issues, architecture deviations |
| 🟢 **SUGGESTION** | Readability, naming, minor best-practice improvements, missing documentation |

## Review Checklist

Work through the following areas and report only findings that apply to the selection:

### Correctness
- Logic errors, off-by-one errors, incorrect null checks
- Race conditions or thread-safety violations
- Incorrect use of `async`/`await` (e.g. `async void`, missing `ConfigureAwait`, fire-and-forget)
- Missing `CancellationToken` pass-through in async code

### Security
- Hardcoded secrets, API keys, passwords
- Missing input validation at system boundaries
- SQL injection risks (raw queries outside EF Core)
- Missing authorization/authentication guards
- Logging of sensitive data

### Code Quality
- Single Responsibility: each method or class should do one thing
- Meaningful names: avoid abbreviations, `x`, `y`, or generic names
- Magic numbers or strings: extract to named constants
- Deep nesting (> 3 levels): prefer guard clauses or early returns
- Code duplication: flag obvious copy-paste

### Error Handling
- Swallowed exceptions (`catch { }`, empty catch blocks)
- Missing null guards for external inputs
- Generic exceptions where specific types are more appropriate
- Missing meaningful error messages or context in log calls

### Performance
- Unnecessary allocations in hot paths
- N+1 query patterns (EF Core: missing `.Include(...)`)
- Blocking calls on async code (`.Result`, `.Wait()`)
- Large collections loaded into memory where paging or streaming is better

### Documentation
- Public APIs without XML documentation (`<summary>`, `<param>`, `<returns>`)
- Non-obvious logic without an explanatory comment

### Testing (when test code is selected)
- Test method name follows `MethodName_Scenario_ExpectedBehavior`
- Clear Arrange-Act-Assert structure (no AAA comments)
- No logic in tests; assertions use xUnit assertion methods (`Assert.*`)
- No shared mutable state between tests

## Comment Format

Use this format for each finding:

```
**[SEVERITY] – Category: Brief title**

Description of the issue and its potential impact.

**Suggested fix:**
```csharp
// concise corrected snippet
```
```

## Examples

### Critical Finding

**🔴 CRITICAL – Security: Hardcoded connection string**

The connection string on line 12 contains credentials that will be committed to source control.

**Suggested fix:**
```csharp
// Instead of:
var conn = "Server=prod;User=sa;Password=secret;";

// Use:
var conn = _configuration.GetConnectionString("Default");
```

### Important Finding

**🟡 IMPORTANT – Correctness: CancellationToken not forwarded**

`GetAsync` accepts a `CancellationToken` but does not pass it to the repository call, making
cancellation ineffective for long-running queries.

**Suggested fix:**
```csharp
// Instead of:
var result = await _repository.GetByIdAsync(id);

// Use:
var result = await _repository.GetByIdAsync(id, cancellationToken);
```

### Suggestion

**🟢 SUGGESTION – Readability: Replace magic number with named constant**

The literal `100` on line 8 has no local context. Extracting it clarifies intent.

**Suggested fix:**
```csharp
private const int MaxPageSize = 100;

// ...
if (request.PageSize > MaxPageSize) { ... }
```

## Closing Note

After listing all findings, add a brief one-line summary:

> **Overall:** _[No blocking issues / N critical issue(s) require attention before merge]_
