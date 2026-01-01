namespace xSdk.Data.Mocks
{
    internal class ConcurrentRepositoryOne : EntityFrameworkRepository<TestDbContext, ConcurrentEntityOne>, IConcurrentRepositoryOne
    {
        public Task AddDataAsync(ConcurrentEntityOne[] samples, CancellationToken token = default)
        {
            return this.InsertAsync(samples, token);
        }

        public Task<IEnumerable<ConcurrentEntityOne>> GetDataAsync(CancellationToken token = default)
        {
            return this.SelectListAsync(token);
        }
    }
}
