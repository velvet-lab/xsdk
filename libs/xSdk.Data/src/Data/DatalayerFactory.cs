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

using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace xSdk.Data;

public class DatalayerFactory(IServiceProvider provider) : IDatalayerFactory
{
    public TRepository CreateRepository<TRepository>()
        where TRepository : IRepository
        => CreateRepository<TRepository>(Globals.DefaultDatalayerName);

    public TRepository CreateRepository<TRepository>(IServiceScope? scope)
        where TRepository : IRepository
        => CreateRepository<TRepository>(Globals.DefaultDatalayerName, scope);

    public TRepository CreateRepository<TRepository>(string? name) where TRepository : IRepository
    { 
        var scope = provider.CreateScope();
        return CreateRepositoryInternal<TRepository>(name, scope);
    }

    public TRepository CreateRepository<TRepository>(string? name, IServiceScope? scope) where TRepository : IRepository
    {
        if (scope == null)
            return CreateRepository<TRepository>(name);
        else
            return CreateRepositoryInternal<TRepository>(name, scope);
    }    

    private TRepository CreateRepositoryInternal<TRepository>(string? name, IServiceScope? scope)
        where TRepository : IRepository
    {
        Guard.IsNotNull(scope);        
        
        TRepository repo = scope.ServiceProvider.GetRequiredKeyedService<TRepository>(name);
            
        if (repo is Repository repository)
        {
            repository.Services = provider;
        }
            
        return repo;        
    }
}
