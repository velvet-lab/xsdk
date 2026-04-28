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
using Microsoft.Extensions.Logging;
using xSdk.Data.Converters.Bson;

namespace xSdk.Data;

public partial class NoSqlRepository<TEntity>
{
    public override Task<TEntity?> SelectAsync(ObjectId primaryKey, CancellationToken token = default)
    {
        _logger.LogTrace("Get Entity by '{0}'...", primaryKey);
        return ExecuteInternalAsync(col => col.FindByIdAsync(BsonValueConverter.Convert(primaryKey)), false, token);
    }

    public override Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default)
    {
        _logger.LogTrace("Get all Entities ...");
        return ExecuteInternalAsync(col => col.FindAllAsync(), false, token);
    }

    protected TEntity Select(Expression<Func<TEntity, bool>> filter) => SelectAsync(filter).GetAwaiter().GetResult();

    protected Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
    {
        _logger.LogTrace("Get Entity");
        return ExecuteInternalAsync(col => col.FindAllAsync(), false, token).ContinueWith(task => task.Result.SingleOrDefault(filter.Compile()), token);
    }

    protected IEnumerable<TEntity> SelectList(Expression<Func<TEntity, bool>> filter) => SelectListAsync(filter).GetAwaiter().GetResult();

    protected Task<IEnumerable<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
    {
        _logger.LogTrace("Get Entities by predicate");

        return ExecuteInternalAsync(col => col.FindAllAsync(), false, token)
            .ContinueWith(
                task =>
                {
                    IEnumerable<TEntity> result = task.Result.Where(filter.Compile()).ToList();
                    return result;
                },
                token
            );
    }
}
