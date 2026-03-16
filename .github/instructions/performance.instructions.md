---
description: 'Performance optimization guidelines for xSDK project'
applyTo: '**/*.cs'
---

# Performance Optimization Guidelines

## General Performance Principles

1. **Measure First**: Profile before optimizing. Don't guess where bottlenecks are.
2. **Simple First**: Write clear code first, optimize when needed
3. **Optimize Hot Paths**: Focus on frequently executed code
4. **Consider Scale**: Think about performance with large data sets
5. **Balance Trade-offs**: Readability vs. performance

## Async/Await Best Practices

### Use Async Throughout

```csharp
// ✅ GOOD: Async all the way
public async Task<User> GetUserAsync(string id, CancellationToken cancellationToken)
{
    var user = await _repository.GetByIdAsync(id, cancellationToken);
    return await EnrichUserDataAsync(user, cancellationToken);
}

// ❌ BAD: Blocking on async (sync-over-async anti-pattern)
public User GetUser(string id)
{
    return _repository.GetByIdAsync(id, default).Result; // Deadlock risk!
}
```

### ConfigureAwait in Library Code

```csharp
// For library code, use ConfigureAwait(false) to avoid capturing context
public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken)
{
    await ValidateAsync(entity).ConfigureAwait(false);
    await _store.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    return entity;
}
```

### Avoid Unnecessary Async

```csharp
// ❌ BAD: Unnecessary async/await
public async Task<User> GetUserAsync(string id)
{
    return await _repository.GetByIdAsync(id, default);
}

// ✅ GOOD: Just return the task
public Task<User> GetUserAsync(string id, CancellationToken cancellationToken)
{
    return _repository.GetByIdAsync(id, cancellationToken);
}
```

## Memory Allocation Optimization

### Use Span<T> and Memory<T>

For performance-critical code working with arrays or strings:

```csharp
// ❌ BAD: Creates substring allocations
public string ExtractMiddle(string input)
{
    return input.Substring(10, 20);
}

// ✅ GOOD: No allocations with Span
public ReadOnlySpan<char> ExtractMiddle(ReadOnlySpan<char> input)
{
    return input.Slice(10, 20);
}
```

### Use ArrayPool for Temporary Buffers

```csharp
using System.Buffers;

// ✅ GOOD: Reuse pooled arrays
public async Task ProcessDataAsync(Stream stream)
{
    var buffer = ArrayPool<byte>.Shared.Rent(4096);
    try
    {
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            ProcessBuffer(buffer.AsSpan(0, bytesRead));
        }
    }
    finally
    {
        ArrayPool<byte>.Shared.Return(buffer);
    }
}
```

### Use StringBuilder for String Concatenation

```csharp
// ❌ BAD: Creates many string instances
public string BuildReport(IEnumerable<string> items)
{
    string report = "";
    foreach (var item in items)
    {
        report += item + "\n";
    }
    return report;
}

// ✅ GOOD: Single allocation with StringBuilder
public string BuildReport(IEnumerable<string> items)
{
    var sb = new StringBuilder();
    foreach (var item in items)
    {
        sb.AppendLine(item);
    }
    return sb.ToString();
}
```

### Avoid Unnecessary Allocations

```csharp
// ❌ BAD: Creates array every time
public bool HasValue(string value)
{
    return new[] { "yes", "true", "1" }.Contains(value);
}

// ✅ GOOD: Static readonly array
private static readonly string[] TrueValues = { "yes", "true", "1" };

public bool HasValue(string value)
{
    return TrueValues.Contains(value);
}

// ✅ EVEN BETTER: Use HashSet for O(1) lookup
private static readonly HashSet<string> TrueValues = new() { "yes", "true", "1" };

public bool HasValue(string value)
{
    return TrueValues.Contains(value);
}
```

## Entity Framework Core Performance

### Avoid N+1 Query Problem

```csharp
// ❌ BAD: N+1 queries (1 for users + N for orders)
public async Task<List<UserWithOrders>> GetUsersWithOrdersAsync()
{
    var users = await _context.Users.ToListAsync();
    foreach (var user in users)
    {
        user.Orders = await _context.Orders
            .Where(o => o.UserId == user.Id)
            .ToListAsync();
    }
    return users;
}

// ✅ GOOD: Single query with Include
public async Task<List<User>> GetUsersWithOrdersAsync()
{
    return await _context.Users
        .Include(u => u.Orders)
        .ToListAsync();
}
```

### Use Projection (Select) to Load Only What You Need

```csharp
// ❌ BAD: Loads entire entity with all properties
public async Task<List<UserDto>> GetUserSummariesAsync()
{
    var users = await _context.Users.ToListAsync();
    return users.Select(u => new UserDto
    {
        Id = u.Id,
        Name = u.Name
    }).ToList();
}

// ✅ GOOD: Projects in database, loads only needed columns
public async Task<List<UserDto>> GetUserSummariesAsync()
{
    return await _context.Users
        .Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name
        })
        .ToListAsync();
}
```

### Use AsNoTracking for Read-Only Queries

```csharp
// ✅ GOOD: No change tracking overhead for read-only
public async Task<List<User>> GetAllUsersAsync()
{
    return await _context.Users
        .AsNoTracking()
        .ToListAsync();
}
```

### Implement Pagination

```csharp
// ✅ GOOD: Paginated queries
public async Task<PagedResult<User>> GetUsersPagedAsync(
    int pageNumber, 
    int pageSize, 
    CancellationToken cancellationToken)
{
    var query = _context.Users.AsQueryable();
    
    var totalCount = await query.CountAsync(cancellationToken);
    
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);
    
    return new PagedResult<User>(items, totalCount, pageNumber, pageSize);
}
```

### Use Compiled Queries for Frequently Executed Queries

```csharp
private static readonly Func<MyDbContext, string, Task<User?>> GetUserByIdQuery =
    EF.CompileAsyncQuery((MyDbContext context, string id) =>
        context.Users.FirstOrDefault(u => u.Id == id));

public Task<User?> GetUserByIdAsync(string id)
{
    return GetUserByIdQuery(_context, id);
}
```

## Collection Performance

### Choose the Right Collection Type

```csharp
// For frequent lookups: HashSet<T> or Dictionary<TKey, TValue>
// For ordered iteration: List<T>
// For unique items: HashSet<T>
// For key-value pairs: Dictionary<TKey, TValue>
// For sorted items: SortedSet<T> or SortedDictionary<TKey, TValue>

// ❌ BAD: O(n) lookup for existence check
public bool UserExists(List<string> userIds, string id)
{
    return userIds.Contains(id); // Linear search
}

// ✅ GOOD: O(1) lookup
public bool UserExists(HashSet<string> userIds, string id)
{
    return userIds.Contains(id); // Hash lookup
}
```

### Use Capacity for Known Sizes

```csharp
// ✅ GOOD: Avoid resizing by setting capacity
public List<User> ProcessUsers(int expectedCount)
{
    var result = new List<User>(capacity: expectedCount);
    // Process users...
    return result;
}
```

## Caching

### Implement Memory Caching

```csharp
public class CachedUserService
{
    private readonly IMemoryCache _cache;
    private readonly IUserRepository _repository;

    public async Task<User?> GetUserAsync(string id, CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{id}";
        
        if (_cache.TryGetValue<User>(cacheKey, out var cachedUser))
        {
            return cachedUser;
        }

        var user = await _repository.GetByIdAsync(id, cancellationToken);
        
        if (user is not null)
        {
            _cache.Set(cacheKey, user, TimeSpan.FromMinutes(10));
        }

        return user;
    }
}
```

### Use Distributed Caching for Scalability

```csharp
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
});
```

## Lazy Initialization

```csharp
public class ExpensiveService
{
    private readonly Lazy<ExpensiveResource> _resource = new(() =>
    {
        // Expensive initialization only when first accessed
        return new ExpensiveResource();
    });

    public ExpensiveResource Resource => _resource.Value;
}
```

## Parallel Processing

Use parallelization for CPU-bound work:

```csharp
// ✅ GOOD: Parallel processing for independent operations
public async Task<List<ProcessedData>> ProcessItemsAsync(
    List<Data> items, 
    CancellationToken cancellationToken)
{
    var options = new ParallelOptions
    {
        MaxDegreeOfParallelism = Environment.ProcessorCount,
        CancellationToken = cancellationToken
    };

    var results = new ConcurrentBag<ProcessedData>();

    await Parallel.ForEachAsync(items, options, async (item, ct) =>
    {
        var processed = await ProcessItemAsync(item, ct);
        results.Add(processed);
    });

    return results.ToList();
}
```

## Streaming Large Data

```csharp
// ✅ GOOD: Stream large files instead of loading into memory
public async Task<IActionResult> DownloadLargeFileAsync(string fileId)
{
    var stream = await _storage.OpenReadStreamAsync(fileId);
    return File(stream, "application/octet-stream", $"{fileId}.dat");
}

// ✅ GOOD: Stream JSON instead of buffering
public async Task<List<Item>> GetLargeDataSetAsync(CancellationToken cancellationToken)
{
    using var response = await _httpClient.GetAsync(
        requestUri, 
        HttpCompletionOption.ResponseHeadersRead, 
        cancellationToken);
    
    await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
    
    return await JsonSerializer.DeserializeAsync<List<Item>>(
        stream, 
        cancellationToken: cancellationToken);
}
```

## Regular Expressions

```csharp
// ❌ BAD: Compiles regex every time
public bool IsValid(string input)
{
    return Regex.IsMatch(input, @"^\d{3}-\d{2}-\d{4}$");
}

// ✅ GOOD: Compiled regex, reused
[GeneratedRegex(@"^\d{3}-\d{2}-\d{4}$", RegexOptions.Compiled)]
private static partial Regex SsnRegex();

public bool IsValid(string input)
{
    return SsnRegex().IsMatch(input);
}
```

## Benchmarking

Use BenchmarkDotNet for accurate performance measurements:

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class StringConcatBenchmark
{
    [Benchmark]
    public string UsingStringConcat()
    {
        string result = "";
        for (int i = 0; i < 100; i++)
            result += i.ToString();
        return result;
    }

    [Benchmark]
    public string UsingStringBuilder()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < 100; i++)
            sb.Append(i);
        return sb.ToString();
    }
}
```

## Performance Testing Checklist

- [ ] Profiled application to identify bottlenecks
- [ ] Async/await used correctly throughout
- [ ] No blocking calls on async code
- [ ] Entity Framework queries are efficient (no N+1)
- [ ] Appropriate collection types used
- [ ] Caching implemented where beneficial
- [ ] Large data sets are paginated
- [ ] Memory allocations minimized in hot paths
- [ ] Regular expressions are compiled
- [ ] Parallel processing used where appropriate
- [ ] Resources are properly disposed
- [ ] Load tested under realistic conditions

## Performance Monitoring

Use Application Insights or OpenTelemetry for production monitoring:

```csharp
// Monitor performance with metrics
_metrics.RecordOperationDuration("user_query", duration);

// Track slow queries
if (duration > TimeSpan.FromSeconds(1))
{
    _logger.LogWarning("Slow query detected: {Duration}ms", duration.TotalMilliseconds);
}
```

Remember: **Premature optimization is the root of all evil.** Optimize when you have data showing it's necessary.
