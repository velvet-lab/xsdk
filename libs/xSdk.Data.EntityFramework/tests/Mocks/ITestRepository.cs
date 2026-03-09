namespace xSdk.Data.Mocks;

internal interface ITestRepository : IRepository
{
    Task AddDataAsync(TestEntity[] samples, CancellationToken token = default);

    Task<IEnumerable<TestEntity>> GetDataAsync(CancellationToken token = default);
}
