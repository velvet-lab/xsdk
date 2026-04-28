/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Text;
using Consul;

namespace xSdk.Data;

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
