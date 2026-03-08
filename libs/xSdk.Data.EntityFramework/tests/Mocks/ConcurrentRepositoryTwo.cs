using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Data.Mocks;

internal class ConcurrentRepositoryTwo : EntityFrameworkRepository<TestDbContext, ConcurrentEntityTwo>, IConcurrentRepositoryTwo
{
    public Task AddDataAsync(ConcurrentEntityTwo[] samples, CancellationToken token = default)
    {
        return this.InsertAsync(samples, token);
    }

    public Task<IEnumerable<ConcurrentEntityTwo>> GetDataAsync(CancellationToken token = default)
    {
        return this.SelectListAsync(token);
    }
}
