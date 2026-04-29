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

using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Data;

public partial class NoSqlRepository<TEntity> : Repository<TEntity, ObjectId>
    where TEntity : class, IEntity<ObjectId>
{
    private readonly object _syncObject = new();
    private static readonly ILogger _logger = LogManager.CreateLogger<NoSqlRepository<TEntity>>();

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

        IDatabase? database = this.DatabaseHandler.Retrieve();

        if (database != null)
        {
            try
            {
                NoSqlDatabaseOptions? setup = database.DatalayerOptions as NoSqlDatabaseOptions;

                collectionName = this.GetTableName();
                openedDatabase = database.Open<LiteDatabaseAsync>();
                if(InitializeIfNeeded(openedDatabase, setup))
                {
                    this.DatabaseHandler.Return(database);
                    database = this.DatabaseHandler.Retrieve();

                    openedDatabase = database.Open<LiteDatabaseAsync>();
                }

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
                _logger.LogWarning(ex, "A Error occurred while execute a Operation with Transactionon the Database");

                if (withTransaction && transactionDatabase != null)
                    await transactionDatabase.RollbackAsync();
            }
            finally
            {
                this.DatabaseHandler.Return(database);
            }
        }

        return result;
    }

    private bool InitializeIfNeeded(ILiteDatabaseAsync? database, NoSqlDatabaseOptions? options)
    {
        lock (_syncObject)
        {
            try
            {
                if (!File.Exists(options.FileName))
                {
                    var collectionName = this.GetTableName();
                    database.UnderlyingDatabase.UpdateIndicies<TEntity>(collectionName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Indicies could not updated for Entity '{0}'", typeof(TEntity).FullName);
            }
        }
        return false;
    }
}
