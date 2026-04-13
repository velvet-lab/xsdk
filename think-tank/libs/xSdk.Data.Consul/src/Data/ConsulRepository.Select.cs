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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace xSdk.Data;

public partial class ConsulRepository<TEntity>
{
    public override Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default) =>
        throw new NotImplementedException("Please use 'SelectListAsync(string prefix, CancellationToken token = default)'");

    protected IEnumerable<TEntity> SelectList(string prefix) => SelectListAsync(prefix).GetAwaiter().GetResult();

    protected Task<IEnumerable<TEntity>> SelectListAsync(string prefix, CancellationToken token = default)
    {
        return this.ExecuteInternalAsync(
            async kv =>
            {
                ICollection<TEntity> entities = new List<TEntity>();

                var queryResult = await kv.List(prefix, token);
                foreach (var result in queryResult.Response)
                {
                    var value = Convert(result.Value);
                    if (!string.IsNullOrEmpty(value))
                    {
                        entities.Add(new TEntity { Key = result.Key, Value = value });
                    }
                }

                return entities as IEnumerable<TEntity>;
            },
            token
        );
    }

    public override Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default)
    {
        return this.ExecuteInternalAsync(
            async kv =>
            {
                TEntity entity = default;

                var queryResult = await kv.Get(primaryKey.GetValue<string>(), token);
                if (queryResult != null)
                {
                    var value = Convert(queryResult.Response.Value);
                    entity.Key = queryResult.Response.Key;
                    entity.Value = value;
                }

                return entity;
            },
            token
        );
    }
}
