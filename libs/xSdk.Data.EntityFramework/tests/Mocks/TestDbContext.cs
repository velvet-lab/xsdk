using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace xSdk.Data.Mocks;

internal class TestDbContext : DbContext
{
    public TestDbContext([NotNull] DbContextOptions<TestDbContext> options)
        : base(options) { }

    public DbSet<TestEntity> TestTable { get; set; }

    public DbSet<ConcurrentEntityOne> ConcurrentTableOne { get; set; }

    public DbSet<ConcurrentEntityTwo> ConcurrentTableTwo { get; set; }
}
