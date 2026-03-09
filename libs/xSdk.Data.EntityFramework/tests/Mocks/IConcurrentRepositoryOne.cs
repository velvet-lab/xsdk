namespace xSdk.Data.Mocks;

internal interface IConcurrentRepositoryOne : IRepository
{
    Task AddDataAsync(ConcurrentEntityOne[] samples, CancellationToken token = default);

    Task<IEnumerable<ConcurrentEntityOne>> GetDataAsync(CancellationToken token = default);
}
