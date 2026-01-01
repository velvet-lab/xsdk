namespace xSdk.Data.Mocks
{
    internal class TestRepository : NoSqlRepository<TestEntity>, ITestRepository
    {
        public Task AddDataAsync(IEnumerable<TestEntity> samples, CancellationToken token = default)
        {
            return this.InsertAsync(samples, token);
        }

        public Task<IEnumerable<TestEntity>> GetDataAsync(CancellationToken token = default)
        {
            return this.SelectListAsync(token);
        }

        public async Task RemoveAll()
        {
            var entities = await this.SelectListAsync();
            foreach (var entity in entities)
            {
                await this.RemoveAsync(entity);
            }
        }
    }
}
