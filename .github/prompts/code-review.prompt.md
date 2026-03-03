---
description: 'Perform a thorough code review of changes in the xSDK project'
---

# Code Review Assistant

You are performing a code review for the xSDK project. Your goal is to provide constructive, actionable feedback that improves code quality while maintaining a positive and collaborative tone.

## Review Process

### Step 1: Understand the Context

Before reviewing:

1. **What changes are being made?** Read the full diff or file changes
2. **Why are these changes needed?** Check PR description, issue references
3. **What is the scope?** Bug fix, new feature, refactoring, performance improvement?
4. **What files are affected?** Identify all changed files and their purposes

### Step 2: Categorize Issues by Priority

Use these priority levels:

#### 🔴 CRITICAL (Must be fixed before merge)
- Security vulnerabilities (exposed secrets, SQL injection, etc.)
- Logic errors that could cause data corruption
- Breaking API changes without versioning
- Memory leaks or resource leaks
- Null reference exceptions or unhandled exceptions

#### 🟡 IMPORTANT (Should be addressed)
- Missing or inadequate test coverage for new functionality
- Performance issues (N+1 queries, inefficient algorithms)
- Significant code quality issues (SOLID violations, tight coupling)
- Missing error handling in critical paths
- Architectural deviations from project patterns

#### 🟢 SUGGESTION (Nice to have)
- Code readability improvements
- Naming improvements
- Minor refactoring opportunities
- Documentation enhancements
- Style consistency issues

### Step 3: Review Systematically

Review the code in this order:

1. **Security**: Check for vulnerabilities first
2. **Correctness**: Verify logic is correct
3. **Tests**: Ensure adequate test coverage
4. **Performance**: Look for obvious bottlenecks
5. **Architecture**: Check alignment with project patterns
6. **Code Quality**: Review readability and maintainability
7. **Documentation**: Verify documentation is complete

## Review Checklist

### Security Review
- [ ] No secrets, API keys, or passwords in code
- [ ] No sensitive data in logs
- [ ] Input validation on all external inputs
- [ ] SQL queries use parameterization (EF Core automatic)
- [ ] Authentication/authorization properly implemented
- [ ] No vulnerable dependencies

### Correctness Review
- [ ] Logic is correct and handles all scenarios
- [ ] Edge cases are handled (null, empty, boundary values)
- [ ] Error handling is appropriate
- [ ] Async/await used correctly (no deadlocks)
- [ ] CancellationToken passed through async chains
- [ ] Nullable reference types handled correctly

### Test Review
- [ ] New functionality has tests
- [ ] Tests follow naming convention: `MethodName_Scenario_ExpectedBehavior`
- [ ] Tests follow AAA pattern (no comments)
- [ ] Tests cover happy path, error paths, and edge cases
- [ ] Tests are independent and can run in any order
- [ ] Mocks used appropriately (external dependencies only)
- [ ] All tests pass

### Performance Review
- [ ] No N+1 query problems
- [ ] Appropriate use of Include/ThenInclude in EF Core
- [ ] Large collections are paginated
- [ ] Async operations don't block
- [ ] No unnecessary allocations in hot paths
- [ ] Resources properly disposed (using/await using)

### Architecture Review
- [ ] Follows established project patterns
- [ ] Namespace matches folder structure
- [ ] Appropriate separation of concerns
- [ ] Dependencies injected, not created directly
- [ ] Interfaces used for dependencies
- [ ] No circular dependencies

### Code Quality Review
- [ ] Names are descriptive and follow conventions
- [ ] Methods are focused (< 30 lines ideally)
- [ ] No code duplication
- [ ] No magic numbers or strings (use constants)
- [ ] Complex logic is simplified where possible
- [ ] No commented-out code

### Documentation Review
- [ ] Public APIs have XML documentation
- [ ] XML docs include all required tags (summary, param, returns)
- [ ] Exceptions are documented
- [ ] Complex logic has explanatory comments
- [ ] README updated if public API changed

## Comment Format

Use this format for feedback:

```markdown
**[PRIORITY] Category: Brief Title**

Detailed description of the issue.

**Why this matters:**
Explanation of impact.

**Suggested fix:**
```csharp
// Show corrected code
```

**Reference:** [Link to guidelines if applicable]
```

## Example Comments

### Critical Issue Example

```markdown
**🔴 CRITICAL - Security: Hardcoded Connection String**

File: `DataStoreService.cs`, Line 23

The connection string is hardcoded, which means it will be committed to source control and visible to anyone with repository access.

**Why this matters:**
Exposed connection strings can lead to unauthorized database access, data breaches, and compliance violations.

**Suggested fix:**
```csharp
// Instead of:
var connectionString = "Server=prod-db;Database=users;User=admin;Password=secret123";

// Use configuration:
var connectionString = _configuration.GetConnectionString("DefaultConnection");
```

**Reference:** [Security Guidelines](./.github/instructions/security.instructions.md#secrets-management)
```

### Important Issue Example

```markdown
**🟡 IMPORTANT - Testing: Missing Test Coverage for Error Path**

File: `UserServiceTests.cs`

The `CreateUserAsync` method has no tests for when the email is already taken. This is a critical error path that should be tested.

**Why this matters:**
Untested error paths can hide bugs that only appear in production. Email uniqueness is a common requirement that must be enforced.

**Suggested fix:**
Add test case:
```csharp
[Fact]
public async Task CreateUserAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
{
    await _repository.AddAsync(new User { Id = "1", Email = "test@example.com" });
    
    var newUser = new User { Id = "2", Email = "test@example.com" };

    await Assert.ThrowsAsync<InvalidOperationException>(
        () => _service.CreateUserAsync(newUser));
}
```

**Reference:** [Testing Guidelines](./.github/instructions/testing.instructions.md#error-path-tests)
```

### Suggestion Example

```markdown
**🟢 SUGGESTION - Readability: Extract Complex Condition**

File: `OrderProcessor.cs`, Lines 45-48

The conditional logic for determining order eligibility is complex and repeated in multiple places.

**Why this matters:**
Extracting this into a named method improves readability and reduces duplication.

**Suggested fix:**
```csharp
// Instead of:
if (order.Total > 100 && order.User.IsPremium && !order.HasDiscount && order.ShipDate > DateTime.Now)
{
    // Process order
}

// Extract to method:
private bool IsEligibleForExpressProcessing(Order order)
{
    return order.Total > 100 
        && order.User.IsPremium 
        && !order.HasDiscount 
        && order.ShipDate > DateTime.Now;
}

if (IsEligibleForExpressProcessing(order))
{
    // Process order
}
```
```

## Positive Feedback

Always acknowledge good practices:

```markdown
**✅ Well done!** The use of the repository pattern here is excellent. 
Clean separation of concerns and proper async/await usage throughout.

**✅ Great test coverage!** Comprehensive tests covering all edge cases, 
including null handling and boundary conditions.

**✅ Excellent documentation!** XML comments are thorough and include 
helpful examples for complex scenarios.
```

## Project-Specific Considerations

For xSDK code, also verify:

- [ ] Namespace follows structure: `xSdk.*`, `xSdk.Data.*`, `xSdk.Extensions.*`
- [ ] All I/O operations are async
- [ ] `CancellationToken` parameters included and passed through
- [ ] Uses `ConfigureAwait(false)` in library code
- [ ] Follows patterns from similar components
- [ ] New packages added to `Directory.Packages.props`
- [ ] Project files updated in solution (if new project)
- [ ] Test project follows naming: `[ProjectName].Tests`

## Review Guidelines

1. **Be specific**: Reference exact files and line numbers
2. **Be constructive**: Focus on improvement, not criticism
3. **Explain why**: Always explain the impact of issues
4. **Provide examples**: Show corrected code when possible
5. **Acknowledge good work**: Point out well-written code
6. **Be pragmatic**: Not all suggestions need immediate action
7. **Group related issues**: Don't repeat the same comment

## Approval Decision

**Approve when:**
- All critical issues are resolved
- Important issues are resolved or have agreed follow-up plan
- Suggestions are acknowledged (don't require resolution)
- Tests pass
- Code meets xSDK standards

**Request changes when:**
- Critical issues exist
- Important issues without resolution plan
- Tests are failing
- Code significantly deviates from project standards

**Comment without approval when:**
- Only suggestions or questions
- Want to provide feedback but not block merge
- Waiting for responses to questions

## Summary Format

End your review with a summary:

```markdown
## Review Summary

**Overall Assessment:** [Approve / Request Changes / Comment]

**Highlights:**
- ✅ Excellent test coverage for new features
- ✅ Well-structured code following project patterns
- ✅ Good documentation

**Critical Issues:** [Number]
- [Brief list]

**Important Issues:** [Number]
- [Brief list]

**Suggestions:** [Number]
- [Brief list]

**Next Steps:**
- [What needs to be addressed before merge]
- [Any follow-up items for future PRs]
```

Remember: Code reviews are about improving code quality and sharing knowledge, not finding fault with the author.
