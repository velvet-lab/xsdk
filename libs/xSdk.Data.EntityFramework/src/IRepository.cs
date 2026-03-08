using Microsoft.EntityFrameworkCore;

namespace xSdk.Data;

public interface IRepository<TDbContext, TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    TDbContext CreateDbContext();

    Task<TDbContext> CreateDbContextAsync(CancellationToken token = default);
}
