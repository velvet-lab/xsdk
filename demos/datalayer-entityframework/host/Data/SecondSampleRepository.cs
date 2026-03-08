using xSdk.Data;

namespace xSdk.Demos.Data;

internal class SecondSampleRepository : EntityFrameworkRepository<SecondDbContext, SecondEntity>, ISecondSampleRepository
{
    public Task AddSamplesAsync(SecondEntity[] samples, CancellationToken token = default)
    {
        return this.InsertAsync(samples, token);
    }

    public Task<IEnumerable<SecondEntity>> GetSamplesAsync(CancellationToken token = default)
    {
        return this.SelectListAsync(token);
    }
}
