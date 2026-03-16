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

using LiteDB.Async;
using NLog;

namespace xSdk.Data;

public partial class NoSqlRepository<TEntity> : Repository<TEntity>
    where TEntity : class, IEntity
{
    private readonly object _syncObject = new();
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private async Task<TResult> ExecuteInternalAsync<TResult>(
        Func<ILiteCollectionAsync<TEntity>, Task<TResult>> func,
        bool withTransaction,
        CancellationToken token
    )
    {
        TResult result = default;
        ILiteDatabaseAsync openedDatabase = null;
        ILiteDatabaseAsync transactionDatabase = null;
        ILiteCollectionAsync<TEntity> col = null;
        string collectionName = default;

        try
        {
            collectionName = this.GetTableName();
            openedDatabase = this.Database.Open<LiteDatabaseAsync>(true);

            if (withTransaction)
            {
                transactionDatabase = await openedDatabase.BeginTransactionAsync();
                col = transactionDatabase.GetCollection<TEntity>(collectionName);
            }
            else
                col = openedDatabase.GetCollection<TEntity>(collectionName);

            result = await func(col);

            if (withTransaction)
            {
                if (transactionDatabase != null)
                    await transactionDatabase.CommitAsync();

                //if (openedDatabase != null)
                //    await openedDatabase.CheckpointAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Warn(ex, "A Error occurred while execute a Operation with Transactionon the Database");

            if (withTransaction && transactionDatabase != null)
                await transactionDatabase.RollbackAsync();
        }

        return result;
    }

    protected override void Initialize()
    {
        lock (_syncObject)
        {
            try
            {
                var database = this.Database.Open<LiteDatabaseAsync>(true);
                var collectionName = GetTableName();
                database.UnderlyingDatabase.UpdateIndicies<TEntity>(collectionName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Indicies could not updated for Entity '{0}'", typeof(TEntity).FullName);
            }
        }
    }
}
