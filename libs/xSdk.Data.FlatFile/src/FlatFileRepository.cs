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
using JsonFlatFileDataStore;

namespace xSdk.Data;

public abstract class FlatFileRepository<TEntity> : Repository<TEntity, int>
    where TEntity : FlatFileEntity
{
    public override Task<bool> InsertAsync(TEntity? entity, CancellationToken token = default)
    {
        if (entity is null)
        {
            return Task.FromResult(false);
        }

        return ExecuteInternalAsync((col) => col.InsertOneAsync(entity), token);
    }

    public override Task<int> InsertAsync(IEnumerable<TEntity>? entities, CancellationToken token = default)
    {
        if (entities is null)
        {
            return Task.FromResult(0);
        }

        return ExecuteInternalAsync((col) => col.InsertManyAsync(entities).ContinueWith(task => entities.Count()), token);
    }

    public override Task<bool> RemoveAsync(int primaryKey, CancellationToken token = default) =>
        ExecuteInternalAsync((col) => col.DeleteOneAsync(x => x.Id == primaryKey), token);

    public override Task<int> RemoveAsync(IEnumerable<int>? primaryKeys, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> RemoveAsync(TEntity? entity, CancellationToken token = default)
    {
        if (entity is null)
        {
            return Task.FromResult(false);
        }

        return ExecuteInternalAsync((col) => col.DeleteOneAsync(x => x.Id == entity.Id), token);
    }

    public override Task<int> RemoveAsync(IEnumerable<TEntity>? entities, CancellationToken token = default)
    {
        if (entities is null)
        {
            return Task.FromResult(0);
        }

        return ExecuteInternalAsync(
            async (col) =>
            {
                int removed = 0;
                foreach (TEntity entity in entities)
                {
                    bool result = await col.DeleteOneAsync(entity.Id);
                    if (result)
                    {
                        removed++;
                    }
                }

                return removed;
            },
            token
        );
    }

    protected Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
        ExecuteInternalAsync(col => col.DeleteManyAsync(ConvertFilter(filter)), token);

    public override Task<TEntity?> SelectAsync(int primaryKey, CancellationToken token = default) =>
        ExecuteInternalAsync((col) => Task.FromResult(col.AsQueryable().SingleOrDefault(x => x.Id == primaryKey)), token);

    public override Task<IEnumerable<TEntity>?> SelectListAsync(CancellationToken token = default)
        => ExecuteInternalAsync((col) => Task.FromResult(col.AsQueryable()), token)
        .ContinueWith(task => task.Result, token);

    protected TEntity? Select(Expression<Func<TEntity, bool>> filter) => SelectAsync(filter).GetAwaiter().GetResult();

    protected Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
        => ExecuteInternalAsync(col => Task.FromResult(col.Find(ConvertFilter(filter))), token)
            .ContinueWith(task => task.Result?.SingleOrDefault(), token);

    protected IEnumerable<TEntity>? SelectList(Expression<Func<TEntity, bool>> filter) => SelectListAsync(filter).GetAwaiter().GetResult();

    protected Task<IEnumerable<TEntity>?> SelectListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
        => ExecuteInternalAsync(col => Task.FromResult(col.Find(ConvertFilter(filter))), token);

    public override Task<bool> UpdateAsync(int primaryKey, TEntity? entity, CancellationToken token = default)
    {
        if(entity is null)
        {
            return Task.FromResult(false);
        }

        return ExecuteInternalAsync((col) => col.UpdateOneAsync(entity.Id, entity), token);
    }

    public override Task<bool> UpsertAsync(TEntity? entity, CancellationToken token = default)
    {
        if(entity is null)
        {
            return Task.FromResult(false);
        }

        return ExecuteInternalAsync(
            async (col) =>
            {
                TEntity? item = col.AsQueryable().SingleOrDefault(x => x.Id == entity.Id);

                bool result = false;
                if (item == null)
                {
                    result = await col.InsertOneAsync(entity);
                }
                else
                {
                    result = await col.UpdateOneAsync(entity.Id, entity);
                }

                return result;
            },
            token
        );
    }

    private async Task<TResult?> ExecuteInternalAsync<TResult>(Func<IDocumentCollection<TEntity>, Task<TResult>> func, CancellationToken token = default)
    {
        TResult? result = default;
        DataStore? openedDatabase;
        IDocumentCollection<TEntity>? col;
        string? collectionName = default;

        IDatabase? database = DatabaseHandler?.Retrieve();

        if (database != null)
        {
            try
            {
                collectionName = GetTableName();
                openedDatabase = database.Open<DataStore>();

                if (openedDatabase != null)
                {
                    col = openedDatabase.GetCollection<TEntity>(collectionName);
                    result = await func(col);
                }
            }
            catch (Exception ex)
            {
                if (token.CanBeCanceled)
                {
                    token.ThrowIfCancellationRequested();
                }

                throw new SdkException("A Error occurred while execute a Operation for the Database", ex);
            }
            finally
            {
                DatabaseHandler?.Return(database);
            }
        }

        return result;
    }

    private static Predicate<TEntity> ConvertFilter(Expression<Func<TEntity, bool>> filter)
    {
        Func<TEntity, bool> compiled = filter.Compile();
        bool pred(TEntity x) => compiled(x);

        return pred;
    }
}
