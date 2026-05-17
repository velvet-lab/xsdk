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

public abstract class EntityFrameworkRepository<TDbContext, TEntity, TPrimaryKeyType> : Repository<TEntity, TPrimaryKeyType>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TPrimaryKeyType>
{
    public override Task<bool> InsertAsync(TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
                await dbContext.AddAsync(entity, token);
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

    public override Task<int> RemoveAsync(IEnumerable<TPrimaryKeyType> primaryKeys, CancellationToken token = default)
        => throw new NotImplementedException();

    public override Task<bool> RemoveAsync(TPrimaryKeyType primaryKey, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
                TEntity? trackedItem = await dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id.Equals(primaryKey), token);
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

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
                TEntity? existing = await SelectAsync(entity.Id, token);
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

    public override Task<int> RemoveAsync(IEnumerable<TEntity>? entities, CancellationToken token = default)
    {
        if(entities == null)
        {
            return Task.FromResult(0);
        }

        return ExecuteInternalAsync(
            (dbContext) =>
            {
                dbContext.RemoveRange(entities);
                return dbContext.SaveChangesAsync(token);
            },
            true,
            token
        );
    }

#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
    public override Task<TEntity?> SelectAsync(TPrimaryKeyType primaryKey, CancellationToken token = default) =>
        ExecuteInternalAsync(
            (dbContext) => dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id.Equals(primaryKey), token),
            false,
            token
        );
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

    protected Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
        ExecuteInternalAsync(dbContext => dbContext.Set<TEntity>().SingleOrDefaultAsync(filter), false, token);

    public override Task<IEnumerable<TEntity>?> SelectListAsync(CancellationToken token = default) =>
        ExecuteInternalAsync(async (dbContext) =>
        {
            DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
            IEnumerable<TEntity> entities = await dbSet.ToListAsync(token);
            entities ??= [];

            return entities;
        }, false, token);

    protected Task<IEnumerable<TEntity>?> SelectListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
        ExecuteInternalAsync(dbContext =>
        {
            var result = dbContext
                .Set<TEntity>()
                .Where(filter)
                .ToListAsync(token) as Task<IEnumerable<TEntity>>;

            return result ?? Task.FromResult<IEnumerable<TEntity>>([]);
        }, false, token);
    public override Task<bool> UpdateAsync(TPrimaryKeyType primaryKey, TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(
            async (dbContext) =>
            {
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
                TEntity? trackedItem = await dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id.Equals(primaryKey), token);
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

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
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
                TEntity? trackedItem = await dbContext
                    .Set<TEntity>()
                    .SingleOrDefaultAsync(x => x.Id.Equals(entity.Id), token);
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.

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

    private async Task<TResult?> ExecuteInternalAsync<TResult>(Func<TDbContext, Task<TResult>> func, bool withTransaction, CancellationToken token)
    {
        TResult? result = default;
        IDbContextTransaction? transaction = null;
        bool shouldUseTransaction = withTransaction;

        IDatabase? database = DatabaseHandler?.Retrieve();

        if (database != null)
        {
            try
            {
                EntityFrameworkDatabaseOptions? setup = GetOptions<EntityFrameworkDatabaseOptions>(OptionsScope.Datalayer);
                if (setup != null && !setup.TransactionsEnabled)
                {
                    shouldUseTransaction = false;
                }

                TDbContext? dbContext = database.Open<TDbContext>();
                if (dbContext != null)
                {

                    // Disable Transactions for MongoDbs, because this feature is not supported
                    if (dbContext.Database.ProviderName == "MongoDB.EntityFrameworkCore")
                    {
                        shouldUseTransaction = false;
                    }

                    if (shouldUseTransaction)
                    {
                        transaction = await dbContext.Database.BeginTransactionAsync(token);
                    }

                    result = await func(dbContext);

                    if (shouldUseTransaction && transaction != null)
                    {
                        await transaction.CommitAsync(token);
                    }
                }
                else
                {
                    throw new SdkException("The Database Object could not be opened");
                }
            }
            catch (Exception ex)
            {
                if (shouldUseTransaction && transaction != null)
                {
                    await transaction.RollbackAsync(token);
                    throw new SdkException("A Error occurred while Operation with Transaction will executed", ex);
                }
                else
                {
                    throw new SdkException("A Error occured while a Operation will executed", ex);
                }
            }
            finally
            {
                DatabaseHandler?.Return(database);
            }
        }

        return result;
    }
}
