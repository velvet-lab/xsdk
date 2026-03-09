using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace xSdk.Demos.Data;

internal class SecondDbContext : DbContext
{
    public SecondDbContext([NotNull] DbContextOptions<SecondDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    public DbSet<SampleEntity> Sample { get; set; }
}
