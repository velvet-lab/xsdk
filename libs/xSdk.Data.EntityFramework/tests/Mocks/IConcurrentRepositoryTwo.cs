using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Data.Mocks;

internal interface IConcurrentRepositoryTwo : IRepository
{
    Task AddDataAsync(ConcurrentEntityTwo[] samples, CancellationToken token = default);

    Task<IEnumerable<ConcurrentEntityTwo>> GetDataAsync(CancellationToken token = default);
}
