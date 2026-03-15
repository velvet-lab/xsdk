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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace xSdk.Data;

public abstract class EntityFrameworkRepository<TDbContext, TEntity> : Repository<TEntity>
    where TDbContext : DbContext
    where TEntity : class, IEntity
{
    protected override EntityFrameworkDatabase<TDbContext> Database => base.Database as EntityFrameworkDatabase<TDbContext>;

    public override Task<bool> InsertAsync(TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                var item = await dbContext.AddAsync(entity, token);
                return await dbContext.SaveChangesAsync(token) > 0;
            },
            true,
            token
        );

    public override Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                await dbContext.AddRangeAsync(entities, token);

                return await dbContext.SaveChangesAsync(token);
            },
            true,
            token
        );

    public override Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                var primaryKeyValue = primaryKey.GetValue();
                var trackedItem = dbContext.Set<TEntity>().SingleOrDefault(x => x.Id == primaryKeyValue);

                if (trackedItem != null)
                {
                    dbContext.Remove(trackedItem);
                    return await dbContext.SaveChangesAsync(token) > 0;
                }
                return false;
            },
            true,
            token
        );

    public override Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                var existing = await SelectAsync(entity.PrimaryKey, token);
                if (existing != null)
                {
                    dbContext.Remove(entity);
                    return await dbContext.SaveChangesAsync(token) > 0;
                }
                return true;
            },
            true,
            token
        );

    public override Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default) =>
        ExecuteInternalAsync(
            (dbContext) =>
            {
                dbContext.RemoveRange(entities);
                return dbContext.SaveChangesAsync(token);
            },
            true,
            token
        );

    public override Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default) =>
        ExecuteInternalAsync(
            (dbContext) =>
            {
                var primaryKeyValue = primaryKey.GetValue();
                return dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == primaryKeyValue, token);
            },
            false,
            token
        );

    protected Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
        ExecuteInternalAsync(dbContext => dbContext.Set<TEntity>().SingleOrDefaultAsync(filter), false, token);

    public override Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                var dbSet = dbContext.Set<TEntity>();
                IEnumerable<TEntity> entities = await dbSet.ToListAsync(token);
                if (entities == null)
                    entities = new List<TEntity>();

                return entities;
            },
            false,
            token
        );

    protected Task<IEnumerable<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
        ExecuteInternalAsync(dbContext => dbContext.Set<TEntity>().Where(filter).ToListAsync() as Task<IEnumerable<TEntity>>, false, token);

    public override Task<bool> UpdateAsync(IPrimaryKey primaryKey, TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                var primaryKeyValue = primaryKey.GetValue();
                var trackedItem = await dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == primaryKeyValue, token);

                if (trackedItem != null)
                {
                    trackedItem = entity.CopyToEntity(trackedItem);
                    dbContext.Update(trackedItem);
                    return await dbContext.SaveChangesAsync(token) > 0;
                }
                else
                {
                    return false;
                }
            },
            true,
            token
        );

    public override Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                var primaryKeyValue = entity.PrimaryKey.GetValue();
                var trackedItem = await dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == primaryKeyValue, token);

                if (trackedItem == null)
                {
                    await dbContext.AddAsync(entity);
                }
                else
                {
                    trackedItem = entity.CopyToEntity(trackedItem);
                    dbContext.Update(trackedItem);
                }

                return await dbContext.SaveChangesAsync(token) > 0;
            },
            true,
            token
        );

    private async Task<TResult> ExecuteInternalAsync<TResult>(Func<TDbContext, Task<TResult>> func, bool withTransaction, CancellationToken token)
    {
        TResult result = default;
        IDbContextTransaction transaction = null;
        var shouldUseTransaction = withTransaction;

        try
        {
            var dbContext = this.Database.Open<TDbContext>(false);
            if (!this.Database.Setup.TransactionsEnabled)
                shouldUseTransaction = false;

            // Disable Transactions for MongoDbs, because this feature is not supported
            if (dbContext.Database.ProviderName == "MongoDB.EntityFrameworkCore")
            {
                shouldUseTransaction = false;
            }

            if (shouldUseTransaction)
                transaction = dbContext.Database.BeginTransaction();

            result = await func(dbContext);

            if (shouldUseTransaction)
            {
                if (transaction != null)
                    await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            if (shouldUseTransaction && transaction != null)
            {
                await transaction.RollbackAsync();
                throw new SdkException("A Error occurred while Operation with Transaction will executed", ex);
            }
            else
                throw new SdkException("A Error occured while a Operation will executed", ex);
        }

        return result;
    }
}
