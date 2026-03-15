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

using System.Linq.Expressions;
using LiteDB;
using xSdk.Data.Converters.Bson;

namespace xSdk.Data;

public partial class NoSqlRepository<TEntity>
{
    public override Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default)
    {
        _logger.Trace("Remove Entity '{0}'", primaryKey);
        return ExecuteInternalAsync(col => col.DeleteAsync(BsonValueConverter.Convert(primaryKey.GetValue<ObjectId>())), true, token);
    }

    public override Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default) => RemoveAsync(entity.PrimaryKey, token);

    public override async Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default)
    {
        _logger.Trace("Remove Entities ...");

        var deleted = 0;
        foreach (var primaryKey in primaryKeys)
        {
            var result = await RemoveAsync(primaryKey, token);
            if (result)
                deleted += 1;
        }

        return deleted;
    }

    public override Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default) =>
        RemoveAsync(entities.Select(x => x.PrimaryKey), token);

    protected Task<int> RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
        ExecuteInternalAsync(col => col.DeleteManyAsync(filter), true, token);
}
