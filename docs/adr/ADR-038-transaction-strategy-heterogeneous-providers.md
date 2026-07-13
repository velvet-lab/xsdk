---
title: "ADR-038: Transaction Strategy for Heterogeneous Data Providers"
status: "Accepted"
date: "2026-07-13"
authors: "xSdk Team"
tags: ["architecture", "data", "transactions", "consistency"]
supersedes: ""
superseded_by: ""
---

# ADR-038: Transaction Strategy for Heterogeneous Data Providers

## Status

Accepted

## Context

The xSdk data layer supports multiple storage backends with fundamentally different transaction semantics:

| Provider                 | Transaction Support | Isolation Level      | Cross-Collection Support |
|--------------------------|---------------------|----------------------|--------------------------|
| EF Core (SQL Server)     | Full ACID           | Configurable         | Yes (same database)      |
| EF Core (MongoDB)        | Limited             | Read Committed       | No (single-document)     |
| FlatFile (JSON)          | None                | N/A                  | N/A                      |
| Vault (Secrets)          | None                | N/A                  | N/A                      |
| LiteDB (NoSQL)           | Limited             | Read Committed       | Yes (same file)          |

Repository implementations ([ADR-037](ADR-037-xsdk-data-foundation-layer.md)) expose a unified CRUD interface. However, when applications call multiple repository methods in sequence, they may expect transactional guarantees that some providers cannot deliver.

**Example scenario:**
```csharp
await customerRepo.InsertAsync(customer);
await orderRepo.InsertAsync(order);
// If the second insert fails, should the first be rolled back?
```

For EF Core, this can be wrapped in a `TransactionScope` or `DbContext.Database.BeginTransaction()`. For MongoDB EF, transactions are not supported. For FlatFile/Vault, transactions are meaningless.

The data layer must:
1. Allow providers to opt into transaction support where possible
2. Fail gracefully (or warn) when consumers request transactions on non-transactional providers
3. Avoid coupling consumer code to provider-specific transaction APIs

## Decision

Each provider declares its transaction capability explicitly, and repository operations accept an optional `withTransaction` flag that is **best-effort** — honored by providers that support it, ignored by those that do not.

### Transaction Capability Declaration

Providers implement `IDatabase` and optionally declare transaction support via a `SupportsTransactions` property (convention, not interface-enforced):

```csharp
public class EntityFrameworkDatabase<TDbContext> : Database, IDatabase
{
    public bool SupportsTransactions => true;
}

public class MongoDbDatabase : Database, IDatabase
{
    public bool SupportsTransactions => false; // MongoDB EF disables transactions
}

public class FlatFileDatabase : Database, IDatabase
{
    public bool SupportsTransactions => false;
}
```

### Repository Execution Pattern

Repository base classes expose `ExecuteInternalAsync<TResult>` or similar methods with a `withTransaction` parameter:

```csharp
protected async Task<TResult> ExecuteInternalAsync<TResult>(
    Func<TConnection, Task<TResult>> func,
    bool withTransaction,
    CancellationToken token)
{
    var db = DatabaseHandler.Retrieve();
    try
    {
        var connection = db.Open<TConnection>();
        
        if (withTransaction && db.SupportsTransactions)
        {
            // Begin transaction (provider-specific)
            // Execute func
            // Commit or rollback
        }
        else
        {
            // Execute func without transaction
            if (withTransaction)
            {
                logger.LogWarning("Transaction requested but not supported by {Provider}", 
                    db.GetType().Name);
            }
        }
        
        return await func(connection);
    }
    finally
    {
        DatabaseHandler.Return(db);
    }
}
```

### Provider-Specific Transaction Implementation

#### EF Core (Transactional)

`EntityFrameworkRepository` wraps operations in `DbContext.Database.BeginTransaction()` when `withTransaction == true`.

```csharp
protected override async Task<TResult> ExecuteAsync<TResult>(
    Func<TDbContext, Task<TResult>> func, 
    bool withTransaction, 
    CancellationToken token)
{
    await using var db = DatabaseHandler.Retrieve();
    await using var context = db.CreateDbContext();
    
    if (withTransaction)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(token);
        try
        {
            var result = await func(context);
            await transaction.CommitAsync(token);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }
    
    return await func(context);
}
```

#### MongoDB EF (Non-Transactional)

`MongoDbContext` sets `AutoTransactionBehavior = AutoTransactionBehavior.Never`. Transactions are not supported. The `withTransaction` flag is ignored.

```csharp
public MongoDbContext(DbContextOptions<MongoDbContext> options)
    : base(options)
{
    Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
}
```

#### FlatFile / Vault (Non-Transactional)

These providers execute operations sequentially without transaction support. The `withTransaction` flag is ignored, and a warning is logged when requested.

### Consumer Best Practices

1. **Single-operation calls:** Most repository methods are atomic within the provider's guarantees. No explicit transaction needed.
2. **Multi-operation workflows:** Use application-level compensation logic or Unit of Work pattern for cross-repository consistency.
3. **Provider-aware code:** Applications that require strong transactional guarantees should validate provider capabilities at startup.

### Cross-Provider Consistency Patterns

When applications need consistency across providers (e.g., update EF Core + write Vault secret), they must implement:

- **Saga Pattern** — compensating transactions for rollback
- **Idempotency** — operations can be safely retried
- **Eventual Consistency** — accept delayed consistency with reconciliation

The data layer does not provide distributed transaction coordination (e.g., 2PC).

## Consequences

### Positive

- **POS-001**: Providers explicitly declare transaction support; consumers can query capability at runtime.
- **POS-002**: Repository API remains provider-agnostic; `withTransaction` is a hint, not a contract.
- **POS-003**: EF Core repositories benefit from transactional safety where supported.
- **POS-004**: Non-transactional providers (FlatFile, Vault) fail gracefully with warnings, not exceptions.
- **POS-005**: Logging makes transaction capability mismatch visible during development and testing.

### Negative

- **NEG-001**: `withTransaction` is best-effort; consumers cannot assume ACID semantics across all providers.
- **NEG-002**: Cross-provider consistency requires manual saga/compensation logic; no framework support.
- **NEG-003**: `SupportsTransactions` is a convention, not an interface contract; forgetting it may cause silent failures.
- **NEG-004**: Applications expecting strong transactional guarantees must validate provider capabilities at startup; runtime discovery is late.
- **NEG-005**: MongoDB's lack of multi-document transactions may surprise developers expecting NoSQL transactional parity with relational databases.

## Alternatives Considered

##### Enforce Transactions via Interface Contract

- **ALT-001**: **Description**: Add `BeginTransaction()`, `Commit()`, `Rollback()` to `IDatabase` interface; require all providers to implement.
- **ALT-002**: **Rejection Reason**: Forces non-transactional providers (FlatFile, Vault) to implement no-op or exception-throwing methods; violates Interface Segregation Principle.

##### Distributed Transaction Coordinator (2PC)

- **ALT-003**: **Description**: Implement a distributed transaction coordinator using `TransactionScope` or custom 2PC.
- **ALT-004**: **Rejection Reason**: Adds significant complexity; requires XA-capable providers; MongoDB/FlatFile/Vault do not support 2PC.

##### Separate Transactional and Non-Transactional Interfaces

- **ALT-005**: **Description**: Define `ITransactionalRepository` and `INonTransactionalRepository` interfaces.
- **ALT-006**: **Rejection Reason**: Splits repository abstraction; consumers must handle two separate contracts, losing provider agnosticism.

## Implementation Notes

- **IMP-001**: Repository base classes log warnings when `withTransaction == true` but provider does not support transactions.
- **IMP-002**: EF Core repositories use `DbContext.Database.BeginTransaction()` explicitly; `TransactionScope` is avoided to prevent distributed transaction promotion.
- **IMP-003**: MongoDB EF provider sets `AutoTransactionBehavior = AutoTransactionBehavior.Never` in `MongoDbContext` constructor.
- **IMP-004**: Applications requiring strong consistency should validate provider transaction support in `Program.cs` startup and fail fast if requirements are not met.

## References

- **REF-001**: [ADR-037](ADR-037-xsdk-data-foundation-layer.md) — xSdk.Data Foundation Layer
- **REF-002**: [ADR-007](ADR-007-entity-framework-data-provider.md) — Entity Framework Core as Relational Data Provider
- **REF-003**: [ADR-011](ADR-011-mongodb-via-efcore.md) — MongoDB Access via EF Core Provider
- **REF-004**: [ADR-009](ADR-009-flatfile-jsonstore-provider.md) — FlatFile JSON Store Provider
- **REF-005**: [ADR-010](ADR-010-vault-secret-management.md) — Vault Secret Management
- **REF-006**: [Saga Pattern](https://microservices.io/patterns/data/saga.html)
