using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace xSdk.Data.Mocks
{
    internal class TestDbContext : DbContext
    {
        public TestDbContext([NotNull] DbContextOptions<TestDbContext> options)
            : base(options) { }

        public DbSet<TestEntity> TestTable { get; set; }

        public DbSet<ConcurrentEntityOne> ConcurrentTableOne { get; set; }

        public DbSet<ConcurrentEntityTwo> ConcurrentTableTwo { get; set; }
    }
}
