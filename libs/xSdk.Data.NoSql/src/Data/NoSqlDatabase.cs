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

namespace xSdk.Data;

public sealed class NoSqlDatabase(ILogger<NoSqlDatabase> logger) : Database
{
    private readonly object _syncObject = new();

    protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder) => Open<TConnection>(null, connectionStringBuilder);

    protected override TConnection Open<TConnection>(object? connection, Func<object> connectionStringBuilder)
    {
        LiteDatabaseAsync database = connection as LiteDatabaseAsync;

        lock (_syncObject)
        {
            try
            {
                if (database == null)
                {
                    var connectionString = connectionStringBuilder();
                    var conStringObj = connectionString as ConnectionString;
                    database = new LiteDatabaseAsync(conStringObj);
                    // _database.Pragma("CHECKPOINT", new BsonValue(0));

                    // After the DB is opened, possible Changes will integrated to the Database
                    database.UnderlyingDatabase.Checkpoint();
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "A asycn LiteDBConnection could not created");
                throw;
            }
        }

        return database as TConnection;
    }

    protected override void Disconnect(object connection)
    {
        lock (_syncObject)
        {
            var database = connection as LiteDatabaseAsync;
            if (database != null)
            {
                try
                {
                    database.UnderlyingDatabase.Checkpoint();
                    database.Dispose();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Async NoSql Database could not closed");
                    throw;
                }
            }
        }
    }
}
