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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Data;

public sealed class DatalayerBuilder(IServiceCollection services) : IDatalayerBuilder
{
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly List<string> _logicalNames = [];
    private readonly Dictionary<object, object> _objectPools = [];

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1873:Potenziell kostspielige Protokollierung vermeiden", Justification = "<Ausstehend>")]
    public IDatabaseBuilder ConfigureDatabase<TDatabase, TDatabaseOptions>(string? name, Action<TDatabaseOptions>? factory)
        where TDatabase : class, IDatabase
        where TDatabaseOptions : class, IVariableSetup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = Globals.DefaultDatalayerName;
        }

        _logger.LogInformation("Registering database with name '{name}'", name);

        TryAddLogicalName(name);

        _logger.LogTrace("Adding database '{name}' to object pool and registering database handler", name);
        AddDatabaseToObjectPool<TDatabase>(name);
        AddDatabaseHandler<TDatabase, TDatabaseOptions>(name);

        _logger.LogTrace("Registering database options for database '{name}'", name);
        services.RegisterOptions<TDatabaseOptions>(name, options => factory?.Invoke(options));

        return new DatabaseBuilder(name, services);
    }

    private void TryAddLogicalName(string name)
    {
        if (_logicalNames.Any(x => string.Compare(x, name, true) == 0))
        {
            throw new SdkException(string.Format("Database with name '{0}' is already registered. Please choose another name to register the database layer", name));
        }

        _logicalNames.Add(name);
    }

    private void AddDatabaseToObjectPool<TDatabase>(string? name)
        where TDatabase : class, IDatabase
    {
        services.TryAddKeyedSingleton<ObjectPool<TDatabase>>(name, (provider, key) =>
        {
            if (!_objectPools.TryGetValue(key, out object? value))
            {
                var policy = new DatabasePoolPolicy<TDatabase>(provider);
                var objectPool = new DefaultObjectPool<TDatabase>(policy);
                _objectPools[key] = objectPool;
                return objectPool;
            }
            else
            {
                return (ObjectPool<TDatabase>)value;
            }
        });
    }

    private void AddDatabaseHandler<TDatabase, TDatabaseOptions>(string? name)
        where TDatabase : class, IDatabase
        where TDatabaseOptions : class, IVariableSetup
    {
        services.TryAddKeyedTransient<IDatabaseHandler>(name, (provider, key) =>
        {
            string datalayerName = (string)key;

            ObjectPool<TDatabase> objectPool = provider.GetRequiredKeyedService<ObjectPool<TDatabase>>(key);
            ILogger<DatabaseHandler<TDatabase>> logger = provider.GetRequiredService<ILogger<DatabaseHandler<TDatabase>>>();

            var poolHandler = new DatabaseHandler<TDatabase>(objectPool, logger)
            {
                DatalayerName = datalayerName,
                Services = provider
            };
            return poolHandler;
        });
    }
}
