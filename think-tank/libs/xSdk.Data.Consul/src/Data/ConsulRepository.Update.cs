using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace xSdk.Data
{
    public partial class ConsulRepository<TEntity>
    {
        public override Task<bool> UpdateAsync(IPrimaryKey primaryKey, TEntity entity, CancellationToken token = default) => UpsertAsync(entity, token);
    }
}
