using xSdk.Data;

namespace xSdk.Demos.Data
{
    internal interface ISampleRepository : IRepository
    {
        Task AddSamplesAsync(SampleEntity[] samples, CancellationToken token = default);

        Task<IEnumerable<SampleEntity>> GetSamplesAsync(CancellationToken token = default);
    }
}
