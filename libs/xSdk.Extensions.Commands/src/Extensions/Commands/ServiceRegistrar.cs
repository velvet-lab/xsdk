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
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using xSdk.Hosting;

namespace xSdk.Extensions.Commands;

public sealed class ServiceRegistrar(IServiceCollection services) : ITypeRegistrar
{
    private IServiceProvider _mainServiceProvider;

    public ITypeResolver Build()
    {
        if(_mainServiceProvider is null)
        {
            throw new InvalidOperationException("Service provider is not built yet.");
        }
        return new ServiceResolver(services.BuildServiceProvider(), _mainServiceProvider);
    }

    internal void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _mainServiceProvider = serviceProvider;
    }

    public void Register(Type service, Type implementation)
    {        
        RegisterNonExistingTypes(service, implementation);
        services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        RegisterNonExistingTypes(service,implementation);
        services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        services.AddSingleton(service, _ => factory());
    }

    // HACK: Registration of non-existent types such as `ICommandModel` and `IConfigurator` to ensure their availability for injection within
    // the `CommandApp` configuration. This is necessary because Spectre.Console.Cli registers certain types only internally. This hack makes
    // these non-publicly registered types available to the DI container.
    private void RegisterNonExistingTypes(Type service, object implementation)
    {
        if (implementation is ICommandModel model && !ExistsNonExistingType<ICommandModel>())
        {
            services.AddSingleton<ICommandModel>(model);
        }

        if (implementation is IConfigurator configurator && !ExistsNonExistingType<IConfigurator>())
        {
            services.AddSingleton<IConfigurator>(configurator);
            if (!ExistsNonExistingType<ICommandAppSettings>())
            {
                services.AddSingleton<ICommandAppSettings>(configurator.Settings);
            }
        }
    }

    private bool ExistsNonExistingType<TType>()
    {
        foreach(var serviceDescriptor in services)
        {
            if (serviceDescriptor.ServiceType == typeof(TType))
            {
                return true;
            }
        }
        return false;
    }
}
