<!-- Inspired by: https://github.com/github/awesome-copilot/blob/main/agents/CSharpExpert.agent.md -->
---
description: 'Testing standards and best practices for xSDK project'
applyTo: '**/*Tests.cs,**/*Tests/**/*.cs'
---

# Testing Best Practices

## Test Project Structure

- Separate test project for each library: `[ProjectName].Tests`
- Test projects mirror the source project structure
- Use the same namespace as the code under test, with `.Tests` suffix
- One test file per class: `MyService.cs` → `MyServiceTests.cs`

## Testing Framework

This project uses **xUnit**:

- Use `[Fact]` for tests with no parameters
- Use `[Theory]` with `[InlineData]` for parameterized tests
- No `[TestFixture]` or `[TestClass]` attributes needed
- Constructor for setup, `IDisposable` for cleanup

## Test Naming Conventions

Use descriptive test names that clearly state what is being tested:

```
MethodName_Scenario_ExpectedBehavior
```

Examples:
- `GetByIdAsync_WhenEntityExists_ReturnsEntity`
- `GetByIdAsync_WhenEntityNotFound_ReturnsNull`
- `SaveAsync_WithNullEntity_ThrowsArgumentNullException`
- `UpdateAsync_WhenConcurrencyConflict_ThrowsDbUpdateConcurrencyException`

## Test Structure (AAA Pattern)

Follow the Arrange-Act-Assert (AAA) pattern without comments:

```csharp
[Fact]
public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
{
    var store = new InMemoryDataStore<User>();
    var user = new User { Id = "1", Name = "John" };
    await store.AddAsync(user);

    var result = await store.GetByIdAsync("1");

    Assert.NotNull(result);
    Assert.Equal("John", result.Name);
}
```

**Important**: Do NOT include "Arrange", "Act", "Assert" comments in actual test code. The blank lines between sections are sufficient.

## Assertions

This project uses **xUnit assertions** as the standard:

```csharp
Assert.Equal(expected, actual);
Assert.NotEqual(expected, actual);
Assert.True(condition);
Assert.False(condition);
Assert.Null(value);
Assert.NotNull(value);
Assert.Empty(collection);
Assert.NotEmpty(collection);
Assert.Contains(item, collection);
Assert.Throws<TException>(() => method());
await Assert.ThrowsAsync<TException>(() => methodAsync());
```

## Parameterized Tests

Use `[Theory]` with `[InlineData]` for testing multiple scenarios:

```csharp
[Theory]
[InlineData("", false)]
[InlineData("  ", false)]
[InlineData("valid", true)]
public void IsValid_WithVariousInputs_ReturnsExpectedResult(string input, bool expected)
{
    var result = Validator.IsValid(input);

    Assert.Equal(expected, result);
}
```

For complex data, use `[MemberData]` or `[ClassData]`:

```csharp
public static IEnumerable<object[]> ValidUserData =>
    new List<object[]>
    {
        new object[] { new User { Id = "1", Name = "John" } },
        new object[] { new User { Id = "2", Name = "Jane" } }
    };

[Theory]
[MemberData(nameof(ValidUserData))]
public async Task SaveAsync_WithValidUser_Succeeds(User user)
{
    // Test implementation
}
```

## Test Isolation and Independence

- Each test must be independent and not rely on other tests
- Tests should be runnable in any order
- Tests should be runnable in parallel
- Use test fixtures (`IClassFixture<T>`) for expensive setup shared across tests in a class
- Clean up resources in `Dispose` or `DisposeAsync`

```csharp
public class DatabaseTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;

    public DatabaseTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        // Clean up test-specific resources
    }
}
```

## Mocking Guidelines

**DO:**
- Mock external dependencies (databases, HTTP clients, file systems)
- Use interfaces for dependencies that need mocking
- Verify mock interactions when behavior is critical

**DON'T:**
- Mock value objects or DTOs
- Mock code that's part of the system under test
- Over-mock - prefer real implementations when lightweight

**Example using Moq:**
```csharp
[Fact]
public async Task GetUserAsync_CallsRepository_ReturnsUser()
{
    var mockRepo = new Mock<IUserRepository>();
    var expectedUser = new User { Id = "1", Name = "John" };
    mockRepo
        .Setup(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedUser);

    var service = new UserService(mockRepo.Object);

    var result = await service.GetUserAsync("1");

    Assert.NotNull(result);
    Assert.Equal(expectedUser.Id, result.Id);
    mockRepo.Verify(r => r.GetByIdAsync("1", It.IsAny<CancellationToken>()), Times.Once);
}
```

## Testing Async Code

- Always use `async Task` for async tests
- Use `await` on all async operations
- Pass `CancellationToken` when the method accepts one
- Test cancellation behavior when applicable

```csharp
[Fact]
public async Task GetDataAsync_WhenCancelled_ThrowsOperationCanceledException()
{
    var cts = new CancellationTokenSource();
    var service = new DataService();

    cts.Cancel();

    await Assert.ThrowsAsync<OperationCanceledException>(
        () => service.GetDataAsync(cts.Token));
}
```

## Edge Cases and Boundary Conditions

Test edge cases and boundary conditions:

- Null values
- Empty collections
- Empty strings
- Maximum and minimum values
- Boundary values (0, -1, int.MaxValue)
- Invalid states
- Concurrent access scenarios

```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("  ")]
public async Task CreateUserAsync_WithInvalidName_ThrowsArgumentException(string name)
{
    var service = new UserService();

    await Assert.ThrowsAsync<ArgumentException>(
        () => service.CreateUserAsync(name));
}
```

## Test Coverage

- All public APIs must have tests
- Critical business logic must have comprehensive tests
- Happy path scenarios
- Error scenarios
- Edge cases
- Target at least 80% code coverage for library code
- Focus on meaningful coverage, not just percentage

## Integration Tests

For integration tests:

- Use `WebApplicationFactory<T>` for testing ASP.NET Core APIs
- Use in-memory database or test containers for database tests
- Clean up resources after each test
- Use realistic test data
- Test complete workflows, not just individual methods

## Test Organization

```
ProjectName.Tests/
├── Unit/
│   ├── Services/
│   │   └── UserServiceTests.cs
│   └── Validators/
│       └── UserValidatorTests.cs
├── Integration/
│   └── Repositories/
│       └── UserRepositoryTests.cs
└── Helpers/
    └── TestDataBuilder.cs
```

## Test Data Builders

Use test data builders for complex objects:

```csharp
public class UserBuilder
{
    private string _id = "1";
    private string _name = "Default User";

    public UserBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public User Build() => new User { Id = _id, Name = _name };
}

// Usage in tests
var user = new UserBuilder()
    .WithId("123")
    .WithName("John Doe")
    .Build();
```

## Running Tests

Run tests locally before committing:

```powershell
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test
dotnet test --filter "FullyQualifiedName~GetByIdAsync_WhenEntityExists"

# Run tests in parallel
dotnet test --parallel
```

## Test Maintenance

- Keep tests simple and focused
- Refactor tests when refactoring code
- Remove obsolete tests
- Update tests when behavior changes
- Regularly review and improve test quality
- Keep test code as clean as production code
