using xSdk.Data;

namespace xSdk.Demos.Data
{
    internal class SampleRepository : EntityFrameworkRepository<SampleDbContext, SampleEntity>, ISampleRepository
    {
        public Task AddSamplesAsync(SampleEntity[] samples, CancellationToken token = default)
        {
            return this.InsertAsync(samples, token);
        }

        public Task<IEnumerable<SampleEntity>> GetSamplesAsync(CancellationToken token = default)
        {
            return this.SelectListAsync(token);
        }
    }
}
