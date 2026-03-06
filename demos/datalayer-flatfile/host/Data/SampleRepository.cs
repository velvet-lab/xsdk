using xSdk.Data;

namespace xSdk.Demos.Data
{
    internal class SampleRepository : FlatFileRepository<SampleEntity>, ISampleRepository
    {
        public Task AddSamplesAsync(SampleEntity[] samples, CancellationToken token = default) =>
            ExecuteAsDemoIfEnabledAsync(repo => repo.InsertAsync(samples, token));

        public Task<IEnumerable<SampleEntity>> GetSamplesAsync(CancellationToken token = default) =>
            ExecuteAsDemoIfEnabledAsync(repo => repo.SelectListAsync(token));

        protected override Task<IEnumerable<SampleEntity>> CreateFakesAsync(CancellationToken token = default) =>
            FakeGenerator.GenerateListAsync<SampleEntityFakes, SampleEntity>(10);
    }
}
