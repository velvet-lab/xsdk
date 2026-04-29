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

namespace xSdk.Data;

public interface IRepository : IDatalayerMetadata
{
}

public interface IRepository<TEntity, TPrimaryKeyType> : IRepository
    where TEntity : class, IEntity<TPrimaryKeyType>
{
    bool Insert(TEntity entity);

    int Insert(IEnumerable<TEntity> entities);

    Task<bool> InsertAsync(TEntity entity, CancellationToken token = default);

    Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    bool Remove(TEntity entity);

    bool Remove(TPrimaryKeyType primaryKey);

    int Remove(IEnumerable<TPrimaryKeyType> primaryKeys);

    int Remove(IEnumerable<TEntity> entities);


    Task<bool> RemoveAsync(TPrimaryKeyType primaryKey, CancellationToken token = default);

    Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default);

    Task<int> RemoveAsync(IEnumerable<TPrimaryKeyType> primaryKeys, CancellationToken token = default);

    Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    TEntity? Select(TPrimaryKeyType primaryKey);

    Task<TEntity?> SelectAsync(TPrimaryKeyType primaryKey, CancellationToken token = default);

    IEnumerable<TEntity> SelectList();

    Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default);

    bool Update(TPrimaryKeyType primaryKey, TEntity entity);

    Task<bool> UpdateAsync(TPrimaryKeyType primaryKey, TEntity entity, CancellationToken token = default);

    bool Upsert(TEntity entity);

    Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default);
}
