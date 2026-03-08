using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace xSdk.Data;

public class MongoDbContext<TContext> : DbContext
    where TContext : DbContext
{
    public MongoDbContext([NotNull] DbContextOptions<TContext> options)
        : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }
}
