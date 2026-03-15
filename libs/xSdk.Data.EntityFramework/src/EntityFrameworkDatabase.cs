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

using Microsoft.EntityFrameworkCore;

namespace xSdk.Data;

public sealed class EntityFrameworkDatabase<TDbContext> : Database
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;

    internal EntityFrameworkDatabaseSetup Setup { get; private set; }

    public EntityFrameworkDatabase(IDbContextFactory<TDbContext> factory)
    {
        this._factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder)
    {
        Setup = connectionStringBuilder() as EntityFrameworkDatabaseSetup;
        return _factory.CreateDbContext() as TConnection;
    }

    protected override TConnection Open<TConnection>(object connection, Func<object> connectionStringBuilder)
    {
        Setup = connectionStringBuilder() as EntityFrameworkDatabaseSetup;
        if (connection == null)
        {
            return _factory.CreateDbContext() as TConnection;
        }
        else
        {
            return connection as TConnection;
        }
    }

    protected override void Disconnect(object connection)
    {
        var dbContext = connection as TDbContext;
        dbContext?.Dispose();
    }
}
