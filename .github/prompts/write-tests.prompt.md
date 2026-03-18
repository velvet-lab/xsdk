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
6. **Check the relevant ADR**: If testing a data provider or repository, read [ADR-005](../../docs/adr/ADR-005-repository-pattern-with-factory.md) and [ADR-006](../../docs/adr/ADR-006-provider-agnostic-data-layer.md). For demo/fake mode behavior, see [ADR-012](../../docs/adr/ADR-012-demo-fake-repository-mode.md). ADR Consequences/Negative sections list known limitations that are intentional — do not write tests that assert against documented trade-offs.

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

    Assert.NotNull(result);
    Assert.Equal(entityId, result.Id);
    Assert.Equal("Test", result.Name);
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

    Assert.NotNull(result);
    Assert.Equal("1", result.Id);

    var saved = await _context.Entities.FindAsync("1");
    Assert.NotNull(saved);
    Assert.Equal("New Entity", saved.Name);
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

    Assert.Empty(result);
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

    Assert.Equal(2, result.Count());
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

    Assert.Equal(expected, result);
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

    Assert.NotNull(result);
    Assert.Equal(user.Id, result.Id);
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

    Assert.NotNull(result);
}
```

## Step 5: Mock Dependencies

Use mocking for external dependencies:

```csharp
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _service = new UserService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetUserAsync_CallsRepository_ReturnsUser()
    {
        var userId = "123";
        var expectedUser = new User { Id = userId, Name = "John" };
        _mockRepository
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var result = await _service.GetUserAsync(userId);

        Assert.Equal(expectedUser, result);
        _mockRepository.Verify(
            r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

## Step 6: Test Coverage Goals

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
- [ ] Mocks used appropriately (Moq)
- [ ] Resources properly disposed

## Common Patterns

### Testing Configuration

```csharp
[Fact]
public void Configure_WithValidOptions_ConfiguresSuccessfully()
{
    var setup = new EntityFrameworkDatabaseSetup
    {
        TransactionsEnabled = true
    };

    Assert.True(setup.TransactionsEnabled);
}
```

### Testing Extension Methods

```csharp
[Fact]
public void AddDatalayer_RegistersServicesCorrectly()
{
    var services = new ServiceCollection();

    services.AddDatalayer(datalayer =>
    {
        datalayer.UseFlatFile("TestDb", config =>
        {
            config.Path = "/tmp";
            config.FileName = "test.json";
        });
    });

    var provider = services.BuildServiceProvider();
    var factory = provider.GetService<IDatalayerFactory>();
    Assert.NotNull(factory);
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

    Assert.Equal(2, results.Count);
}
