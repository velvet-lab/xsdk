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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace xSdk.Data;

internal abstract class DatabaseHandler() : IDatabaseHandler
{
    public string? DatalayerName { get; internal set; }

    public IServiceProvider? Services { get; internal set; }

    public abstract IDatabase? Retrieve();

    public abstract void Return(IDatabase? database);
}

internal class DatabaseHandler<TDatabase>(ObjectPool<TDatabase> pool, ILogger<DatabaseHandler<TDatabase>> logger) : DatabaseHandler(), IDatabaseHandler<TDatabase>
    where TDatabase : class, IDatabase
{
    public override IDatabase? Retrieve()
    {
        logger.LogTrace("Try to open Database");

        TDatabase database = pool.Get();
        if (database is Database concreteDatabase)
        {
            concreteDatabase.DatalayerName = DatalayerName;
            concreteDatabase.Services = Services;
        }

        return database;
    }

    public override void Return(IDatabase? database)
    {
        if (database != null)
        {
            logger.LogTrace("Try to close Database");
            pool.Return((TDatabase)database);
        }
    }
}
