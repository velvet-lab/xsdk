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

namespace xSdk.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatalayer(this IServiceCollection services)
    {
        return services.AddDatalayer((builder) => { });
    }

    public static IServiceCollection AddDatalayer(this IServiceCollection services, Action<IDatalayerBuilder> configure)
    {
        return services.AddSingleton<IDatalayerFactory>(_ =>
        {
            var builder = new DatalayerBuilder(services);
            configure?.Invoke(builder);

            var factory = new DatalayerFactory();
            factory.Provider = services.BuildServiceProvider();

            return factory;
        });
    }

    public static IDatalayerBuilder UseDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(this IDatalayerBuilder builder, string name)
        where TDatabase : class, IDatabase
        where TDatabaseSetup : IDatabaseSetup, new()
        where TConnectionStringBuilder : class, IConnectionBuilder => builder.ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(name);

    public static IDatalayerBuilder UseDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(
        this IDatalayerBuilder builder,
        string name,
        Action<TDatabaseSetup> configure
    )
        where TDatabase : class, IDatabase
        where TDatabaseSetup : IDatabaseSetup, new()
        where TConnectionStringBuilder : class, IConnectionBuilder =>
        builder.ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(name, configure);

    public static IDatalayerBuilder MapRepository<TImplementation>(this IDatalayerBuilder builder, params string[] dataProviders)
        where TImplementation : class, IRepository => builder.ConfigureRepository<TImplementation>(dataProviders);

    public static IDatalayerBuilder MapRepository<TInterface, TImplementation>(this IDatalayerBuilder builder, params string[] dataProviders)
        where TInterface : class
        where TImplementation : class, IRepository, TInterface => builder.ConfigureRepository<TInterface, TImplementation>(dataProviders);
}
