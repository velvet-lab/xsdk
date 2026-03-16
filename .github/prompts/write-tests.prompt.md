---
description: 'Generate comprehensive tests for xSDK code'
---

# Test Generation

You are helping to write comprehensive tests for xSDK code.

## Step 1: Understand the Code Under Test

Before writing tests:

1. **Identify the component**: What class/method are you testing?
2. **Understand behavior**: What does it do?
3. **Identify dependencies**: What external dependencies does it have?
4. **Review edge cases**: What are the boundary conditions and error scenarios?
5. **Check existing tests**: Look for existing test patterns in the project

## Step 2: Determine Test Scope

Identify what needs to be tested:

- **Public methods**: All public methods must have tests
- **Happy path**: Normal, expected usage
- **Error paths**: Invalid inputs, exception scenarios
- **Edge cases**: Null values, empty collections, boundary values
- **Async operations**: Cancellation, timeouts
- **Integration points**: Interaction with dependencies

## Step 3: Set Up Test Project

Ensure test project structure:

```
ProjectName.Tests/
├── ProjectName.Tests.csproj
└── [MirrorSourceStructure]/
    └── ClassNameTests.cs
```

Test file name: `{ClassName}Tests.cs`

## Step 4: Write Tests Using xUnit

### Test Class Structure

```csharp
namespace xSdk.Data.EntityFramework.Tests;

public class EntityFrameworkDataStoreTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly EntityFrameworkDataStore<TestEntity> _store;

    public EntityFrameworkDataStoreTests()
    {
        // Arrange: Set up test dependencies
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new TestDbContext(options);
        _store = new EntityFrameworkDataStore<TestEntity>(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    // Tests go here
}
```

### Test Naming Convention

Use the pattern: `MethodName_Scenario_ExpectedBehavior`

Examples:
- `GetByIdAsync_WhenEntityExists_ReturnsEntity`
- `GetByIdAsync_WhenEntityNotFound_ReturnsNull`
- `SaveAsync_WithNullEntity_ThrowsArgumentNullException`
- `UpdateAsync_WithValidEntity_UpdatesSuccessfully`

### Test Structure (AAA Pattern)

```csharp
[Fact]
public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
{
    var entityId = "test-123";
    var entity = new TestEntity { Id = entityId, Name = "Test" };
    await _context.Entities.AddAsync(entity);
    await _context.SaveChangesAsync();

    var result = await _store.GetByIdAsync(entityId);

    result.Should().NotBeNull();
    result.Id.Should().Be(entityId);
    result.Name.Should().Be("Test");
}
```

**Important**: Do NOT add "Arrange", "Act", "Assert" comments. Use blank lines to separate sections.

### Happy Path Tests

Test normal, expected usage:

```csharp
[Fact]
public async Task SaveAsync_WithValidEntity_SavesSuccessfully()
{
    var entity = new TestEntity { Id = "1", Name = "New Entity" };

    var result = await _store.SaveAsync(entity);

    result.Should().NotBeNull();
    result.Id.Should().Be("1");
    
    var saved = await _context.Entities.FindAsync("1");
    saved.Should().NotBeNull();
    saved.Name.Should().Be("New Entity");
}
```

### Error Path Tests

Test invalid inputs and error scenarios:

```csharp
[Fact]
public async Task SaveAsync_WithNullEntity_ThrowsArgumentNullException()
{
    await Assert.ThrowsAsync<ArgumentNullException>(
        () => _store.SaveAsync(null!));
}

[Fact]
public async Task GetByIdAsync_WithEmptyId_ThrowsArgumentException()
{
    await Assert.ThrowsAsync<ArgumentException>(
        () => _store.GetByIdAsync(string.Empty));
}

[Fact]
public async Task UpdateAsync_WhenEntityNotFound_ThrowsInvalidOperationException()
{
    var entity = new TestEntity { Id = "non-existent", Name = "Test" };

    await Assert.ThrowsAsync<InvalidOperationException>(
        () => _store.UpdateAsync(entity));
}
```

### Edge Case Tests

Test boundary conditions:

```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public async Task GetByIdAsync_WithInvalidId_ThrowsArgumentException(string invalidId)
{
    await Assert.ThrowsAsync<ArgumentException>(
        () => _store.GetByIdAsync(invalidId));
}

[Fact]
public async Task GetAllAsync_WhenNoEntities_ReturnsEmptyCollection()
{
    var result = await _store.GetAllAsync();

    result.Should().BeEmpty();
}

[Fact]
public async Task SearchAsync_WithEmptySearchTerm_ReturnsAllEntities()
{
    await _context.Entities.AddRangeAsync(
        new TestEntity { Id = "1", Name = "First" },
        new TestEntity { Id = "2", Name = "Second" }
    );
    await _context.SaveChangesAsync();

    var result = await _store.SearchAsync(string.Empty);

    result.Should().HaveCount(2);
}
```

### Parameterized Tests

Use `[Theory]` for testing multiple scenarios:

```csharp
[Theory]
[InlineData("", false)]
[InlineData("  ", false)]
[InlineData("valid@email.com", true)]
[InlineData("invalid-email", false)]
public void IsValidEmail_WithVariousInputs_ReturnsExpectedResult(
    string email, 
    bool expected)
{
    var result = EmailValidator.IsValid(email);

    result.Should().Be(expected);
}
```

For complex data, use `[MemberData]`:

```csharp
public static IEnumerable<object[]> ValidUserData =>
    new List<object[]>
    {
        new object[] { new User { Id = "1", Name = "John", Email = "john@test.com" } },
        new object[] { new User { Id = "2", Name = "Jane", Email = "jane@test.com" } }
    };

[Theory]
[MemberData(nameof(ValidUserData))]
public async Task SaveAsync_WithValidUsers_SavesSuccessfully(User user)
{
    var result = await _store.SaveAsync(user);

    result.Should().NotBeNull();
    result.Id.Should().Be(user.Id);
}
```

### Async and Cancellation Tests

Test cancellation token behavior:

```csharp
[Fact]
public async Task GetByIdAsync_WhenCancelled_ThrowsOperationCanceledException()
{
    var cts = new CancellationTokenSource();
    cts.Cancel();

    await Assert.ThrowsAsync<OperationCanceledException>(
        () => _store.GetByIdAsync("123", cts.Token));
}

[Fact]
public async Task SaveAsync_WithCancellationToken_PropagatesToken()
{
    var entity = new TestEntity { Id = "1", Name = "Test" };
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

    var result = await _store.SaveAsync(entity, cts.Token);

    result.Should().NotBeNull();
}
```

## Step 5: Mock Dependencies

Use mocking for external dependencies:

```csharp
public class UserServiceTests
{
    private readonly IUserRepository _mockRepository;
    private readonly ILogger<UserService> _mockLogger;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = Substitute.For<IUserRepository>();
        _mockLogger = Substitute.For<ILogger<UserService>>();
        _service = new UserService(_mockRepository, _mockLogger);
    }

    [Fact]
    public async Task GetUserAsync_CallsRepository_ReturnsUser()
    {
        var userId = "123";
        var expectedUser = new User { Id = userId, Name = "John" };
        _mockRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(expectedUser);

        var result = await _service.GetUserAsync(userId);

        result.Should().Be(expectedUser);
        await _mockRepository.Received(1)
            .GetByIdAsync(userId, Arg.Any<CancellationToken>());
    }
}
```

## Step 6: Use FluentAssertions

Prefer FluentAssertions when available:

```csharp
// Instead of:
Assert.NotNull(result);
Assert.Equal("expected", result.Name);
Assert.True(result.IsActive);

// Use:
result.Should().NotBeNull();
result.Name.Should().Be("expected");
result.IsActive.Should().BeTrue();

// Collections:
result.Should().HaveCount(3);
result.Should().Contain(x => x.Id == "1");
result.Should().BeInAscendingOrder(x => x.Name);

// Exceptions:
await action.Should().ThrowAsync<ArgumentNullException>()
    .WithMessage("*userId*");
```

## Step 7: Test Coverage Goals

Aim for comprehensive coverage:

- All public methods
- All branches (if/else, switch)
- Exception handling
- Edge cases
- Integration points

Run coverage report:

```powershell
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=cobertura
```

## Guidelines

- **One behavior per test**: Each test should verify one specific behavior
- **Independent tests**: Tests should not depend on each other
- **Fast tests**: Keep tests fast; avoid unnecessary delays
- **Descriptive names**: Test names should clearly describe what is being tested
- **Arrange-Act-Assert**: Follow the AAA pattern (without comments)
- **Mock external dependencies**: Don't mock domain logic
- **Test the public API**: Don't test private methods directly
- **Realistic test data**: Use realistic data, not overly simplified
- **Clean up**: Dispose resources properly

## Checklist

Before submitting tests:

- [ ] All public methods have tests
- [ ] Happy path scenarios covered
- [ ] Error paths covered
- [ ] Edge cases covered
- [ ] Cancellation token behavior tested
- [ ] Tests use descriptive names
- [ ] Tests follow AAA pattern
- [ ] No "Arrange/Act/Assert" comments
- [ ] Tests are independent
- [ ] All tests pass
- [ ] Code coverage is adequate (>80% for new code)
- [ ] FluentAssertions used (if available)
- [ ] Mocks used appropriately
- [ ] Resources properly disposed

## Common Patterns

### Testing Configuration

```csharp
[Fact]
public void Configure_WithValidOptions_ConfiguresSuccessfully()
{
    var options = new DataStoreOptions
    {
        ConnectionString = "test-connection",
        TimeoutSeconds = 30
    };

    var configured = Options.Create(options);

    configured.Value.ConnectionString.Should().Be("test-connection");
    configured.Value.TimeoutSeconds.Should().Be(30);
}
```

### Testing Extension Methods

```csharp
[Fact]
public void AddDataStore_RegistersServicesCorrectly()
{
    var services = new ServiceCollection();

    services.AddDataStore<TestEntity>();

    var provider = services.BuildServiceProvider();
    var store = provider.GetService<IDataStore<TestEntity>>();
    store.Should().NotBeNull();
}
```

### Testing Async Streams

```csharp
[Fact]
public async Task GetAllAsyncStream_ReturnsAllEntities()
{
    await _context.Entities.AddRangeAsync(
        new TestEntity { Id = "1" },
        new TestEntity { Id = "2" }
    );
    await _context.SaveChangesAsync();

    var results = new List<TestEntity>();
    await foreach (var entity in _store.GetAllAsyncStream())
    {
        results.Add(entity);
    }

    results.Should().HaveCount(2);
}
```
