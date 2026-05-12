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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace xSdk.Data;

public sealed class EntityFrameworkDatabase<TDbContext>(IDbContextFactory<TDbContext> factory, ILogger<EntityFrameworkDatabase<TDbContext>> logger) : Database(logger)
    where TDbContext : DbContext
{
    private TDbContext? _dbContext = null;

    public override TDatabaseObject? Open<TDatabaseObject>() where TDatabaseObject : class
    {
        _dbContext ??= factory.CreateDbContext();

        return _dbContext as TDatabaseObject;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1873:Potenziell kostspielige Protokollierung vermeiden", Justification = "<Ausstehend>")]
    public override bool Close()
    {
        if (_dbContext != null)
        {
            logger.LogInformation("Closing Entity Framework database connection for datalayer '{DatalayerName}'.", DatalayerName);
            _dbContext.Dispose();
            _dbContext = null;
            return true;
        }
        else
        {
            logger.LogWarning("Attempted to close Entity Framework database connection for datalayer '{DatalayerName}', but no active connection was found.", DatalayerName);
            return false;
        }
    }
}
