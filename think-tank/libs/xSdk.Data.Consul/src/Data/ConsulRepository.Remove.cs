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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace xSdk.Data;

public partial class ConsulRepository<TEntity>
{
    public override Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default) => RemoveAsync(entity.PrimaryKey, token);

    public override async Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default)
    {
        var result = 0;

        foreach (var primaryKey in primaryKeys)
        {
            var success = await RemoveAsync(primaryKey, token);
            if (success)
                result++;
        }

        return result;
    }

    public override Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default) =>
        RemoveAsync(entities.Select(x => x.PrimaryKey), token);

    public override Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default)
    {
        return this.ExecuteInternalAsync(
            kv =>
            {
                return kv.Delete(primaryKey.GetValue<string>(), token).ContinueWith(task => task.Result.Response, token);
            },
            token
        );
    }
}
