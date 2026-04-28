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
using xSdk.Extensions.Options;
using xSdk.Extensions.Variable;

namespace xSdk.Data;

internal class DatabaseBuilder(string name, IServiceCollection services) : IDatabaseBuilder
{
    public IDatabaseBuilder ConfigureOptions<TOptions>(string? name, Action<TOptions>? factory)
        where TOptions : class, IVariableSetup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = Globals.DefaultDatalayerName;
        }

        services.RegisterOptions(name, factory);

        return this;
    }

    public IDatabaseBuilder ConfigureRepository<TImplementation>()
        where TImplementation : class, IRepository
    {
        services.TryAddKeyedScoped(name, (provider, key) =>
        {
            TImplementation instance = ActivatorUtilities.CreateInstance<TImplementation>(provider);
            if (instance is Repository repository)
            {
                repository.DatalayerName = (string)key;
            }
            return instance;
        });

        return this;
    }

    public IDatabaseBuilder ConfigureRepository<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, IRepository, TInterface
    {
        services.TryAddKeyedScoped<TInterface>(name, (provider, key) =>
        {
            TImplementation instance = ActivatorUtilities.CreateInstance<TImplementation>(provider);
            if (instance is Repository repository)
            {
                repository.DatalayerName = (string)key;
            }

            return instance;
        });

        return this;
    }
}
