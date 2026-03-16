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

public class DatalayerFactory : IDatalayerFactory
{
    internal IServiceProvider Provider;

    public TRepository CreateRepository<TRepository>(string name)
        where TRepository : IRepository
    {
        var scope = Provider.CreateScope();
        return CreateRepositoryInternal<TRepository>(scope, name);
    }

    public TRepository CreateRepository<TRepository>(IServiceScope scope, string name)
        where TRepository : IRepository
    {
        if (scope == null)
            return CreateRepository<TRepository>(name);
        else
            return CreateRepositoryInternal<TRepository>(scope, name);
    }

    private TRepository CreateRepositoryInternal<TRepository>(IServiceScope scope, string name)
        where TRepository : IRepository
    {
        var provider = scope.ServiceProvider;

        var repo = provider.GetRequiredService<TRepository>();

        var abstractRepo = repo as Repository;
        var setups = abstractRepo.InternalSetups;

        if (!setups.Any())
            throw new SdkException("No Database Setup found.");

        if (string.IsNullOrEmpty(name) && setups.Count() > 1)
            throw new SdkException("More than one Database configured. Please specify the Datalayer or Database Name to create a Repository");

        if (!string.IsNullOrEmpty(name) && !setups.Any(x => string.Compare(x.Name, name, true) == 0))
            throw new SdkException($"No Database Setup found for Name '{name}'");

        InternalDatabaseSetup databaseSetup = default;
        if (string.IsNullOrEmpty(name))
            databaseSetup = setups.FirstOrDefault();
        else
            databaseSetup = setups.SingleOrDefault(x => string.Compare(x.Name, name, true) == 0);

        var database = provider.GetRequiredService(databaseSetup.DatabaseType) as IDatabase;
        var connectionBuilder = provider.GetRequiredService(databaseSetup.ConnectionBuilderType) as IConnectionBuilder;

        ((Database)database).Configure(connectionBuilder, databaseSetup);

        abstractRepo.Configure(database);

        return repo;
    }
}
