using xSdk.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace xSdk.Demos.Data
{
    internal class SampleDbContext : MongoDbContext<SampleDbContext>
    {
        public SampleDbContext([NotNull] DbContextOptions<SampleDbContext> options)
            : base(options) { }

        public DbSet<SampleEntity> Sample { get; set; }
    }
}
