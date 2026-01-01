using xSdk.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Demos.Data
{
    internal interface ISecondSampleRepository : IRepository
    {
        Task AddSamplesAsync(SecondEntity[] samples, CancellationToken token = default);

        Task<IEnumerable<SecondEntity>> GetSamplesAsync(CancellationToken token = default);
    }
}
