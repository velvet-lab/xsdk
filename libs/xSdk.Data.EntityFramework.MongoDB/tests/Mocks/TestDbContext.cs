using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace xSdk.Data.Mocks
{
    internal class TestDbContext : MongoDbContext<TestDbContext>
    {
        public TestDbContext([NotNull] DbContextOptions<TestDbContext> options)
            : base(options) { }

        public DbSet<TestEntity> TestTable { get; set; }
    }
}
