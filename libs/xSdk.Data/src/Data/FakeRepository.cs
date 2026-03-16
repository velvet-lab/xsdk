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

using System.Collections.ObjectModel;

namespace xSdk.Data;

public sealed class FakeRepository<TEntity> : Repository<TEntity>
    where TEntity : class, IEntity
{
    private ICollection<TEntity> _dbValues;

    internal FakeRepository(IEnumerable<TEntity> values)
    {
        _dbValues = new Collection<TEntity>(values.ToList()) ?? throw new ArgumentNullException(nameof(values));
    }

    public override Task<bool> InsertAsync(TEntity entity, CancellationToken token = default)
    {
        _dbValues.Add(entity);
        return Task.FromResult(true);
    }

    public override Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        _dbValues = _dbValues.Concat(entities).ToList();

        return Task.FromResult(entities.Count());
    }

    public override Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default)
    {
        _dbValues = _dbValues.Where(x => x.PrimaryKey != primaryKey).ToList();

        return Task.FromResult(true);
    }

    public override Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default)
    {
        _dbValues.Remove(entity);

        return Task.FromResult(true);
    }

    public override Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        foreach (var entity in entities)
            Remove(entity);

        return Task.FromResult(entities.Count());
    }

    public override Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default)
    {
        foreach (var primaryKey in primaryKeys)
            Remove(primaryKey);

        return Task.FromResult(primaryKeys.Count());
    }

    public override Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default)
    {
        return Task.FromResult(_dbValues.SingleOrDefault(x => x.PrimaryKey == primaryKey));
    }

    public override Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default)
    {
        return Task.FromResult<IEnumerable<TEntity>>(_dbValues);
    }

    public override Task<bool> UpdateAsync(IPrimaryKey primaryKey, TEntity entity, CancellationToken token = default)
    {
        Remove(primaryKey);
        Insert(entity);

        return Task.FromResult(true);
    }

    public override Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default)
    {
        return UpdateAsync(entity.PrimaryKey, entity, token);
    }
}
