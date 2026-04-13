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

namespace xSdk.Data;

public sealed class NoSqlDatabase(IOptionsMonitor<NoSqlDatabaseOptions> options, ILogger<NoSqlDatabase> logger) : Database(logger)
{
    private readonly object _syncObject = new();
    private LiteDatabaseAsync? _database;
    private ConnectionString _connectionString;

    public override TDatabaseObject? Open<TDatabaseObject>() where TDatabaseObject : class
    {
        lock (_syncObject)
        {
            try
            {
                if (_database == null)
                {
                    var connectionString = CreateConnectionString();
                    _database = new LiteDatabaseAsync(connectionString);
                    // _database.Pragma("CHECKPOINT", new BsonValue(0));

                    // After the DB is opened, possible Changes will integrated to the Database
                    _database.UnderlyingDatabase.Checkpoint();
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "A asycn LiteDBConnection could not created");
                throw;
            }
        }

        return _database as TDatabaseObject;
    }

    public override bool Close()
    {
        lock (_syncObject)
        {
            if (_database != null)
            {
                try
                {
                    _database.UnderlyingDatabase.Checkpoint();
                    _database.Dispose();
                    _database = null;                    

                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Async NoSql Database could not closed");
                    throw;
                }
            }
        }

        return false;
    }

    private ConnectionString? CreateConnectionString()
    {
        if (_connectionString == null)
        {
            var concreteSetup = options.Get(this.DatalayerName);

            // Only Direct Mode will work correct
            var mode = ConnectionType.Direct;

            ConnectionString? connectionString = new ConnectionString()
            {
                Upgrade = concreteSetup.Upgrade,
                Connection = mode,
                ReadOnly = concreteSetup.ReadOnly,
                Collation = concreteSetup.Collation,
            };

            if (!string.IsNullOrEmpty(concreteSetup.FileName))
            {
                var fileName = concreteSetup.FileName;
                fileName = ResolvePlaceholders(fileName);

                if (!fileName.EndsWith(".db"))
                    fileName += ".db";

                connectionString.Filename = fileName;
            }

            if (!string.IsNullOrEmpty(concreteSetup.Password))
                connectionString.Password = concreteSetup.Password;

            if (concreteSetup.InitialSize > 0)
                connectionString.InitialSize = concreteSetup.InitialSize;

            var fileInfo = new FileInfo(connectionString.Filename);
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            _connectionString = connectionString;
        }

        return _connectionString;
    }

    
}
