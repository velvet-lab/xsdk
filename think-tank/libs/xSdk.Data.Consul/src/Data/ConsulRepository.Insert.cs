using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace xSdk.Data
{
    public partial class ConsulRepository<TEntity>
    {
        public override Task<bool> InsertAsync(TEntity entity, CancellationToken token = default) => UpsertAsync(entity, token);

        public override async Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            var result = 0;
            foreach (var entity in entities)
            {
                var success = await InsertAsync(entity, token);
                if (success)
                    result++;
            }
            return result;
        }
    }
}
