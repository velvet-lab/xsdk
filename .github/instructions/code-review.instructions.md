<!-- Based on: https://github.com/github/awesome-copilot/blob/main/instructions/code-review-generic.instructions.md -->
---
description: 'Code review standards and best practices for xSDK project'
applyTo: '**'
excludeAgent: ["coding-agent"]
---

# Code Review Standards

## Review Language

When performing a code review, respond in **English**.

## Review Priorities

Categorize issues by severity:

### 🔴 CRITICAL (Block merge)
- **Security**: Vulnerabilities, exposed secrets, authentication/authorization issues
- **Correctness**: Logic errors, data corruption risks, race conditions
- **Breaking Changes**: API contract changes without versioning
- **Data Loss**: Risk of data loss or corruption

### 🟡 IMPORTANT (Requires discussion)
- **Code Quality**: Severe violations of SOLID principles, excessive duplication
- **Test Coverage**: Missing tests for critical paths or new functionality
- **Performance**: Obvious performance bottlenecks (N+1 queries, memory leaks)
- **Architecture**: Significant deviations from established patterns

### 🟢 SUGGESTION (Non-blocking improvements)
- **Readability**: Poor naming, complex logic that could be simplified
- **Optimization**: Performance improvements without functional impact
- **Best Practices**: Minor deviations from conventions
- **Documentation**: Missing or incomplete comments/documentation

## General Review Principles

1. **Be specific**: Reference exact files and lines, provide concrete examples
2. **Provide context**: Explain WHY something is an issue and the potential impact
3. **Suggest solutions**: Show corrected code when applicable
4. **Be constructive**: Focus on improving the code, not criticizing the author
5. **Recognize good practices**: Acknowledge well-written code and smart solutions
6. **Be pragmatic**: Not every suggestion needs immediate implementation
7. **Group related comments**: Avoid multiple comments about the same topic

## Code Quality Standards

### Clean Code

- Descriptive and meaningful names for classes, methods, variables
- Single Responsibility Principle: each unit does one thing well
- DRY (Don't Repeat Yourself): no code duplication
- Methods should be focused (ideally < 30 lines)
- Avoid deeply nested code (max 3-4 levels)
- Avoid magic numbers and strings (use constants)
- Code should be self-documenting

Example:
```csharp
// ❌ BAD: Poor naming, magic numbers
public async Task<User> Get(string x, int y)
{
    if (y > 100) return await _repo.GetPremium(x);
    return await _repo.Get(x);
}

// ✅ GOOD: Clear naming, constants
private const int PremiumUserThreshold = 100;

public async Task<User> GetUserByIdAsync(string userId, int accountLevel)
{
    if (accountLevel > PremiumUserThreshold)
        return await _repository.GetPremiumUserAsync(userId);

    return await _repository.GetUserAsync(userId);
}
```

### Error Handling

- Proper error handling at appropriate levels
- Meaningful error messages with context
- No silent failures or ignored exceptions
- Fail fast: validate inputs early
- Use appropriate exception types

Example:
```csharp
// ❌ BAD: Silent failure and generic error
public async Task ProcessUserAsync(string userId)
{
    try
    {
        var user = await _repository.GetByIdAsync(userId);
        await user.ProcessAsync();
    }
    catch
    {
        // Silent failure!
    }
}

// ✅ GOOD: Explicit error handling with context
public async Task ProcessUserAsync(string userId, CancellationToken cancellationToken)
{
    ArgumentException.ThrowIfNullOrEmpty(userId);

    try
    {
        var user = await _repository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException($"User {userId} not found");
        }

        await user.ProcessAsync(cancellationToken);
    }
    catch (Exception ex) when (ex is not InvalidOperationException)
    {
        _logger.LogError(ex, "Failed to process user {UserId}", userId);
        throw new ProcessingException($"Failed to process user {userId}", ex);
    }
}
```

## Security Review

Check for security issues:

- **Secrets**: No passwords, API keys, tokens in code or logs
- **Input Validation**: All user inputs are validated and sanitized
- **SQL Injection**: Use parameterized queries (EF Core does this automatically)
- **Authentication**: Proper authentication checks before accessing resources
- **Authorization**: Verify user has permission to perform action
- **Cryptography**: Use established libraries, never roll your own
- **Dependencies**: Check for known vulnerabilities

Example:
```csharp
// ❌ BAD: Exposed secret
var apiKey = "sk_live_abc123xyz789";

// ✅ GOOD: Use configuration
var apiKey = _configuration["ApiSettings:ApiKey"];
```

## Testing Standards

Verify test quality:

- **Coverage**: Critical paths and new functionality must have tests
- **Test Names**: Descriptive names explaining what is being tested
- **Test Structure**: Clear Arrange-Act-Assert pattern
- **Independence**: Tests should not depend on each other
- **Assertions**: Use specific assertions
- **Edge Cases**: Test boundary conditions, null values, empty collections
- **Mock Appropriately**: Mock external dependencies, not domain logic

Example:
```csharp
// ✅ GOOD: Descriptive name and clear test
[Fact]
public async Task GetUserByIdAsync_WhenUserExists_ReturnsUser()
{
    var userId = "123";
    var expectedUser = new User { Id = userId, Name = "John" };
    await _repository.AddAsync(expectedUser);

    var result = await _repository.GetByIdAsync(userId);

    Assert.NotNull(result);
    Assert.Equal(userId, result.Id);
    Assert.Equal("John", result.Name);
}
```

## Performance Considerations

Check for performance issues:

- **Database Queries**: Avoid N+1 queries, use proper indexing
- **Algorithms**: Appropriate time/space complexity for the use case
- **Caching**: Utilize caching for expensive or repeated operations
- **Resource Management**: Proper cleanup of connections, files, streams
- **Pagination**: Large result sets should be paginated
- **Lazy Loading**: Load data only when needed

Example:
```csharp
// ❌ BAD: N+1 query problem
var users = await _context.Users.ToListAsync();
foreach (var user in users)
{
    user.Orders = await _context.Orders
        .Where(o => o.UserId == user.Id)
        .ToListAsync();
}

// ✅ GOOD: Use Include for efficient loading
var users = await _context.Users
    .Include(u => u.Orders)
    .ToListAsync();
```

## Architecture and Design

Verify architectural principles:

- **Separation of Concerns**: Clear boundaries between layers
- **Dependency Direction**: Follow dependency injection patterns
- **Interface Segregation**: Small, focused interfaces
- **Loose Coupling**: Components independently testable
- **High Cohesion**: Related functionality grouped together
- **Consistent Patterns**: Follow established patterns in the codebase

## Documentation Standards

Check documentation:

- **XML Documentation**: Public APIs must have XML documentation
- **Complex Logic**: Non-obvious logic should have explanatory comments
- **README Updates**: Update README when adding features or changing setup
- **Breaking Changes**: Document any breaking changes clearly
- **Examples**: Provide usage examples for complex features

## Comment Format Template

Use this format for code review comments:

```markdown
**[PRIORITY] Category: Brief title**

Detailed description of the issue or suggestion.

**Why this matters:**
Explanation of the impact or reason for the suggestion.

**Suggested fix:**
```csharp
// Code example
```

**Reference:** [link to relevant documentation or standard]
```

### Example Comments

#### Critical Issue
```markdown
**🔴 CRITICAL - Security: Exposed API Key**

Line 45 contains a hardcoded API key that would be committed to source control.

**Why this matters:**
Exposed API keys can be used by anyone with repository access, leading to unauthorized
API usage and potential data breaches.

**Suggested fix:**
```csharp
// Instead of:
var apiKey = "sk_live_abc123xyz789";

// Use:
var apiKey = _configuration["ApiSettings:ApiKey"];
```

**Reference:** [Security Guidelines](./.github/instructions/security.instructions.md)
```

#### Important Issue
```markdown
**🟡 IMPORTANT - Testing: Missing test coverage for error path**

The `CreateUserAsync` method has no tests for the scenario when validation fails.

**Why this matters:**
Error paths are just as critical as happy paths. Untested error handling can lead
to unhandled exceptions in production.

**Suggested fix:**
Add test case:
```csharp
[Fact]
public async Task CreateUserAsync_WithInvalidEmail_ThrowsArgumentException()
{
    var invalidUser = new User { Email = "not-an-email" };

    await Assert.ThrowsAsync<ArgumentException>(
        () => _service.CreateUserAsync(invalidUser));
}
```
```

#### Suggestion
```markdown
**🟢 SUGGESTION - Readability: Simplify conditional logic**

Lines 30-35 contain nested conditionals that make the logic hard to follow.

**Why this matters:**
Simpler code is easier to maintain, debug, and test.

**Suggested fix:**
```csharp
// Instead of nested ifs:
if (user is not null)
{
    if (user.IsActive)
    {
        if (user.HasPermission("write"))
        {
            await ProcessAsync(user);
        }
    }
}

// Use guard clauses:
if (user is null || !user.IsActive || !user.HasPermission("write"))
{
    return;
}

await ProcessAsync(user);
```
```

## Review Checklist

Systematically verify:

### Code Quality
- [ ] Code follows project conventions and style
- [ ] Names are descriptive
- [ ] Methods are small and focused
- [ ] No code duplication
- [ ] Complex logic is simplified
- [ ] Error handling is appropriate
- [ ] No commented-out code

### Security
- [ ] No secrets in code or configuration
- [ ] Input validation on all user inputs
- [ ] No SQL injection vulnerabilities
- [ ] Authentication and authorization properly implemented

### Testing
- [ ] New code has appropriate test coverage
- [ ] Tests are well-named and focused
- [ ] Tests cover edge cases and error scenarios
- [ ] Tests are independent

### Performance
- [ ] No obvious performance issues (N+1, memory leaks)
- [ ] Appropriate use of async/await
- [ ] Efficient algorithms and data structures
- [ ] Proper resource cleanup

### Architecture
- [ ] Follows established patterns
- [ ] Proper separation of concerns
- [ ] Dependencies flow correctly

### Documentation
- [ ] Public APIs are documented with XML comments
- [ ] Complex logic has explanatory comments
- [ ] README is updated if needed
- [ ] Breaking changes are documented

## xSDK-Specific Considerations

When reviewing xSDK code, also check:

- [ ] Follows namespace structure (`xSdk.Data.*`, `xSdk.Extensions.*`)
- [ ] All I/O operations are async
- [ ] `CancellationToken` passed through async methods
- [ ] Nullable reference types handled correctly
- [ ] Interfaces prefixed with "I"
- [ ] Test project follows naming convention (`[ProjectName].Tests`)
- [ ] New packages added to `Directory.Packages.props`
- [ ] Uses existing patterns from the codebase

## Approval Guidelines

**Approve when:**
- All critical issues are resolved
- Important issues are resolved or have agreed-upon follow-up
- Suggestions are acknowledged (resolution not required)
- Tests pass
- Code meets project standards

**Request changes when:**
- Critical issues exist
- Important issues without agreement on resolution
- Tests are missing or failing
- Code significantly deviates from project standards

Remember: Code reviews are about improving code quality and sharing knowledge, not finding fault.
