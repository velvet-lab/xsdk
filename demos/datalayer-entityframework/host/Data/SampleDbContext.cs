using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace xSdk.Demos.Data;

internal class SampleDbContext : DbContext
{
    public SampleDbContext([NotNull] DbContextOptions<SampleDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    public DbSet<SampleEntity> Sample { get; set; }
}
