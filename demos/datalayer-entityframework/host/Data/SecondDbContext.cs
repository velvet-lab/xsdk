using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace xSdk.Demos.Data
{
    internal class SecondDbContext : DbContext
    {
        public SecondDbContext([NotNull] DbContextOptions<SecondDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public DbSet<SampleEntity> Sample { get; set; }
    }
}
