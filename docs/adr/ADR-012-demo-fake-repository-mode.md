# ADR-012: Demo Mode with In-Memory Fake Repository

## Status

Accepted

## Date

2026-03-17

## Context

During development, sales demos, trade shows, or environments where no real database is available, applications must be able to operate with synthetic data without code changes. Switching to a "demo mode" must:

1. Be transparent to repository consumers — no conditional logic in business code.
2. Support seeding with realistic fake data.
3. Be activated purely through configuration (environment variable or setup property).

## Decision

Two components implement demo mode:

### FakeRepository\<TEntity\>

An in-memory `Repository<TEntity>` that uses a `Collection<TEntity>` as backing store. It implements the full CRUD interface:

- `InsertAsync` → `_dbValues.Add(entity)`
- `SelectAsync(pk)` → `_dbValues.SingleOrDefault(x => x.PrimaryKey == pk)`
- `SelectListAsync()` → `IEnumerable<TEntity>` over the collection
- `UpdateAsync` → find + replace in collection
- `UpsertAsync` → insert if not found, update if found
- `RemoveAsync` → remove from collection

`FakeRepository<TEntity>` is `sealed` and `internal` to the SDK; it cannot be instantiated directly by application code.

### Demo Mode Activation

`IsDemoMode` is a property on `Repository` base class:

```csharp
protected bool IsDemoMode
{
    get
    {
        var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
        return envSetup?.IsDemo ?? false;
    }
}
```

`IsDemo` is wired to the `IEnvironmentSetup.IsDemo` variable, which can be set via:
- Environment variable: `{APP_PREFIX}_ENVIRONMENT_DEMO=true`
- `appsettings.json`: `"environment": { "demo": true }`
- Programmatic: `envSetup.IsDemo = true`

### Demo Data Seeding

`Repository<TEntity>` provides a virtual `CreateFakesAsync` method that returns an empty list by default:

```csharp
protected virtual Task<IEnumerable<TEntity>> CreateFakesAsync(CancellationToken token = default) =>
    Task.FromResult<IEnumerable<TEntity>>(new List<TEntity>());
```

Concrete repositories override this to return Bogus-generated fake entities:

```csharp
protected override Task<IEnumerable<Customer>> CreateFakesAsync(CancellationToken token)
{
    var faker = new Faker<Customer>()
        .RuleFor(x => x.Name, f => f.Company.CompanyName())
        .RuleFor(x => x.Email, f => f.Internet.Email());

    return Task.FromResult(faker.Generate(20).Cast<Customer>());
}
```

### Transparent Delegation

Every CRUD call in `Repository<TEntity>` routes through:

```csharp
protected Task<TResult> ExecuteAsDemoIfEnabledAsync<TResult>(
    Func<Repository<TEntity>, Task<TResult>> concreteCall,
    CancellationToken token = default)
{
    if (IsDemoMode)
    {
        if (_fakeRepository == null)
        {
            var items = CreateFakesAsync(token).GetAwaiter().GetResult();
            _fakeRepository = new FakeRepository<TEntity>(items);
        }
        return concreteCall(_fakeRepository);
    }
    return concreteCall(this);
}
```

The `FakeRepository` is lazily initialized on first use and reused for the repository's lifetime. Business code calls `repository.SelectListAsync()` — it has no knowledge of whether it is talking to a real database or a fake.

### FakeGenerator

`FakeGenerator` (backed by `Bogus`) is a utility class provided in `xSdk.Data` for generating typed collections of fake entities using Bogus' `Faker<T>` API.

## Consequences

### Positive

- Zero code changes required for demo mode — purely configuration-driven.
- `CreateFakesAsync` override keeps fake data close to the repository that understands the entity structure.
- The `FakeRepository` is fully functional for all CRUD operations — demos can create, update, and delete records (within the in-memory state).

### Negative

- `FakeRepository` state is per-repository-instance and per-process — restarting the application resets all data.
- `CreateFakesAsync` is called synchronously (via `.GetAwaiter().GetResult()`) inside `ExecuteAsDemoIfEnabledAsync` on first use, which can cause thread pool starvation if called in a high-concurrency async context.
- If `IsDemoMode` toggles after the fake repository is initialized, the toggle is not respected (the fake repo is cached).
