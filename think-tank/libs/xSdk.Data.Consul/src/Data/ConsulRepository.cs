using System.Text;
using Consul;

namespace xSdk.Data
{
    public partial class ConsulRepository<TEntity> : Repository<TEntity>
        where TEntity : ConsulEntity, new()
    {
        private async Task<TResult> ExecuteInternalAsync<TResult>(Func<IKVEndpoint, Task<TResult>> func, CancellationToken token)
        {
            TResult result = default;
            IConsulClient client = null;

            try
            {
                client = this.Database.Open<IConsulClient>();
                result = await func(client.KV);
            }
            catch (Exception ex)
            {
                throw new SdkException("A Error occurred while execute a Operation with Transactionon the Database", ex);
            }
            finally
            {
                if (client != null)
                    client.Dispose();
            }

            return result;
        }

        private string? Convert(byte[] value)
        {
            if (value != null)
                return Encoding.UTF8.GetString(value);

            return default;
        }

        private byte[]? Convert(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Encoding.UTF8.GetBytes(value);

            return null;
        }
    }
}
