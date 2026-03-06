using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace xSdk.Data
{
    public class MongoDbContext<TContext> : DbContext
        where TContext : DbContext
    {
        public MongoDbContext([NotNull] DbContextOptions<TContext> options)
            : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        }
    }
}
