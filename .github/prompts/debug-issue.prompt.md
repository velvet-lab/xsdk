<!-- Based on: https://github.com/github/awesome-copilot/blob/main/agents/debug.agent.md -->
---
description: 'Systematically debug and resolve issues in xSDK code'
---

# Debugging Assistant

You are helping to debug an issue in the xSDK project. Your goal is to systematically identify the root cause and provide a reliable solution.

## Phase 1: Understand the Problem

### Step 1: Gather Information

Collect all available information about the issue:

1. **What is the problem?**
   - What is the expected behavior?
   - What is the actual behavior?
   - Is it consistent or intermittent?

2. **When does it occur?**
   - Under what conditions?
   - With what inputs?
   - In what environment?

3. **What changed recently?**
   - New code?
   - Configuration changes?
   - Dependency updates?

4. **Error details:**
   - Error messages?
   - Stack traces?
   - Log entries?
   - Exception details?

### Step 2: Reproduce the Issue

Before debugging, reproduce the issue reliably:

```csharp
// Create a minimal reproducible example
[Fact]
public async Task ReproduceIssue()
{
    // Arrange: Set up the exact scenario that causes the issue
    var store = new EntityFrameworkDataStore<User>(_context);
    var user = new User { Id = null! }; // null ID causes error

    // Act & Assert: Expect the error to occur
    var exception = await Assert.ThrowsAsync<ArgumentException>(
        () => store.GetByIdAsync(user.Id));

    // Document what you observe
    _testOutputHelper.WriteLine($"Exception: {exception.Message}");
}
```

## Phase 2: Investigate Root Cause

### Step 3: Analyze the Code Path

Trace the execution path leading to the error:

1. **Identify entry point**: Where does the problematic flow start?
2. **Follow the path**: What methods are called?
3. **Check state**: What is the state of variables at each step?
4. **Find the failure point**: Where exactly does it fail?

### Step 4: Use Debugging Tools

**Console Output:**
```csharp
Console.WriteLine($"User ID: {userId}");
Console.WriteLine($"Store state: {_store.GetState()}");
```

**Debug Logging:**
```csharp
_logger.LogDebug("Processing user {UserId} with status {Status}", 
    user.Id, user.Status);
```

**Breakpoints and inspection:**
- Set breakpoints at critical points
- Inspect variable values
- Watch expressions
- Step through code line by line

### Step 5: Form Hypotheses

Based on investigation, form specific hypotheses:

1. **Hypothesis 1**: Null reference - user object is null
2. **Hypothesis 2**: Database connection issue - context is disposed
3. **Hypothesis 3**: Concurrency issue - multiple threads accessing shared state

Test each hypothesis systematically.

## Phase 3: Common Issue Patterns

### Pattern 1: Null Reference Exceptions

**Symptom:** `NullReferenceException` or `ArgumentNullException`

**Check for:**
```csharp
// Missing null checks
public async Task ProcessUserAsync(User user) // ❌ user could be null
{
    await _repository.SaveAsync(user);
}

// Missing null-conditional operator
var email = user.Email.ToLower(); // ❌ Email could be null

// Uninitialized properties
public string Name { get; set; } // ❌ Could be null
```

**Fix:**
```csharp
public async Task ProcessUserAsync(User user)
{
    ArgumentNullException.ThrowIfNull(user);
    await _repository.SaveAsync(user);
}

var email = user.Email?.ToLower() ?? string.Empty;

public string Name { get; set; } = string.Empty;
```

### Pattern 2: Async/Await Issues

**Symptom:** Deadlocks, `Task` never completes, unexpected blocking

**Check for:**
```csharp
// Sync over async antipattern
public User GetUser(string id)
{
    return _repository.GetByIdAsync(id).Result; // ❌ Deadlock risk!
}

// Not awaiting async calls
public async Task SaveUserAsync(User user)
{
    _repository.SaveAsync(user); // ❌ Fire and forget!
    // continues before save completes
}

// Blocking in async  
public async Task ProcessAsync()
{
    Thread.Sleep(1000); // ❌ Blocks thread
}
```

**Fix:**
```csharp
public async Task<User> GetUserAsync(string id, CancellationToken cancellationToken = default)
{
    return await _repository.GetByIdAsync(id, cancellationToken);
}

public async Task SaveUserAsync(User user, CancellationToken cancellationToken = default)
{
    await _repository.SaveAsync(user, cancellationToken);
}

public async Task ProcessAsync(CancellationToken cancellationToken = default)
{
    await Task.Delay(1000, cancellationToken);
}
```

### Pattern 3: Entity Framework Issues

**Symptom:** Tracking errors, disposed context, N+1 queries

**Check for:**
```csharp
// Disposed context
public async Task<User> GetUserAsync(string id)
{
    using var context = new AppDbContext();
    var user = await context.Users.FindAsync(id);
    return user; // ❌ Navigation properties won't load
}

// N+1 query problem
var users = await context.Users.ToListAsync();
foreach (var user in users)
{
    // ❌ Separate query for each user's orders
    var orders = await context.Orders
        .Where(o => o.UserId == user.Id)
        .ToListAsync();
}

// Tracking conflicts
context.Users.Add(existingUser); // ❌ Already tracked
```

**Fix:**
```csharp
// Proper context lifetime (via DI)
public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserAsync(string id)
    {
        return await _context.Users
            .Include(u => u.Orders)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}

// Eager loading to avoid N+1
var users = await context.Users
    .Include(u => u.Orders)
    .ToListAsync();

// AsNoTracking for read-only
var user = await context.Users
    .AsNoTracking()
    .FirstOrDefaultAsync(u => u.Id == id);
```

### Pattern 4: Concurrency Issues

**Symptom:** Intermittent failures, race conditions, inconsistent state

**Check for:**
```csharp
// Shared mutable state
private static int _counter = 0;

public void Increment()
{
    _counter++; // ❌ Not thread-safe
}

// Concurrent collection access
private List<User> _users = new();

public void AddUser(User user)
{
    _users.Add(user); // ❌ Not thread-safe
}
```

**Fix:**
```csharp
// Use thread-safe types
private int _counter = 0;

public void Increment()
{
    Interlocked.Increment(ref _counter);
}

// Use concurrent collections
private ConcurrentBag<User> _users = new();

public void AddUser(User user)
{
    _users.Add(user);
}

// Or use locks
private readonly object _lock = new();
private List<User> _users = new();

public void AddUser(User user)
{
    lock (_lock)
    {
        _users.Add(user);
    }
}
```

### Pattern 5: Configuration Issues

**Symptom:** `ConfigurationException`, missing settings, wrong values

**Check for:**
```csharp
// Hardcoded configuration
var connectionString = "Server=localhost;..."; // ❌

// Missing configuration check
var apiKey = configuration["ApiKey"]; // ❌ Could be null
UseApiKey(apiKey);
```

**Fix:**
```csharp
// Use IOptions pattern
services.Configure<DatabaseOptions>(
    configuration.GetSection("Database"));

public class DataService
{
    private readonly DatabaseOptions _options;

    public DataService(IOptions<DatabaseOptions> options)
    {
        _options = options.Value;
        
        // Validate configuration
        if (string.IsNullOrEmpty(_options.ConnectionString))
        {
            throw new InvalidOperationException(
                "Database connection string not configured");
        }
    }
}
```

## Phase 4: Implement Fix

### Step 6: Create a Failing Test

Before fixing, write a test that reproduces the bug:

```csharp
[Fact]
public async Task GetByIdAsync_WithNullId_ShouldThrowArgumentException()
{
    // This test currently fails due to the bug
    var store = new EntityFrameworkDataStore<User>(_context);

    await Assert.ThrowsAsync<ArgumentException>(
        () => store.GetByIdAsync(null!));
}
```

### Step 7: Implement the Fix

Make targeted, minimal changes:

```csharp
public async Task<TEntity?> GetByIdAsync(
    string id, 
    CancellationToken cancellationToken = default)
{
    // Fix: Add null/empty validation
    ArgumentException.ThrowIfNullOrEmpty(id);
    
    return await _context.Set<TEntity>()
        .FindAsync(new object[] { id }, cancellationToken);
}
```

### Step 8: Verify the Fix

1. **Run the failing test** - it should now pass
2. **Run all tests** - ensure no regressions
3. **Test manually** - verify in realistic scenario
4. **Check edge cases** - test boundary conditions

```powershell
# Run specific test
dotnet test --filter "GetByIdAsync_WithNullId_ShouldThrowArgumentException"

# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## Phase 5: Prevent Recurrence

### Step 9: Add Defensive Programming

Add checks to prevent similar issues:

```csharp
public async Task<User> ProcessUserAsync(User user, CancellationToken cancellationToken)
{
    // Validate early
    ArgumentNullException.ThrowIfNull(user);
    ArgumentException.ThrowIfNullOrEmpty(user.Id);
    
    // Log for debugging
    _logger.LogDebug("Processing user {UserId}", user.Id);
    
    try
    {
        return await _repository.SaveAsync(user, cancellationToken);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to process user {UserId}", user.Id);
        throw;
    }
}
```

### Step 10: Document the Fix

Document what was fixed and why:

```csharp
/// <summary>
/// Retrieves an entity by its unique identifier.
/// </summary>
/// <param name="id">
/// The unique identifier. Must not be null or empty.
/// </param>
/// <exception cref="ArgumentException">
/// Thrown when <paramref name="id"/> is null or empty.
/// </exception>
/// <remarks>
/// Fixed in v1.2.3: Added validation to prevent null reference exception
/// when id parameter is null or empty. See issue #123.
/// </remarks>
public async Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken)
{
    ArgumentException.ThrowIfNullOrEmpty(id);
    return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
}
```

## Debugging Checklist

- [ ] Issue clearly understood and documented
- [ ] Issue reproduced reliably
- [ ] Root cause identified
- [ ] Failing test written
- [ ] Fix implemented
- [ ] All tests pass
- [ ] Manual testing completed
- [ ] Edge cases tested
- [ ] Defensive code added
- [ ] Fix documented
- [ ] Similar issues checked in codebase

## Debugging Tools

**Visual Studio / Rider:**
- Breakpoints
- Conditional breakpoints
- Data breakpoints
- Exception breakpoints
- Immediate window
- Watch window

**Logging:**
```csharp
_logger.LogDebug("Debug info");
_logger.LogInformation("General info");
_logger.LogWarning("Warning");
_logger.LogError(exception, "Error occurred");
```

**Diagnostics:**
```csharp
using System.Diagnostics;

var sw = Stopwatch.StartNew();
// Code to measure
sw.Stop();
_logger.LogInformation("Operation took {Elapsed}ms", sw.ElapsedMilliseconds);
```

Remember: Understand the problem completely before attempting to fix it. A well-understood problem is half solved.
