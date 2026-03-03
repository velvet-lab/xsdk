using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Consul;

namespace xSdk.Data
{
    public partial class ConsulRepository<TEntity>
    {
        public override Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default)
        {
            return this.ExecuteInternalAsync(
                kv =>
                {
                    var pair = new KVPair(entity.Key);
                    pair.Value = Convert(entity.Value);

                    return kv.Put(pair).ContinueWith(task => task.Result.Response, token);
                },
                token
            );
        }
    }
}
