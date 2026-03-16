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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Hosting;

namespace xSdk.Extensions.Variable;

public static class SerivceCollectionExtensions
{
    private static bool _isLocked;

    private static readonly List<Action<VariableServiceSetup>> _configureActions = new List<Action<VariableServiceSetup>>();

    public static IServiceCollection AddVariableServices(this IServiceCollection services) => services.AddVariableServices(null);

    public static IServiceCollection AddVariableServices(this IServiceCollection services, Action<VariableServiceSetup>? configure)
    {
        if (configure != null)
        {
            _configureActions.Add(configure);
        }

        services.TryAddSingleton(provider =>
        {
            var service = SlimHost.Instance.VariableSystem;

            var setup = new VariableServiceSetup();
            foreach (var configureAction in _configureActions)
            {
                configureAction?.Invoke(setup);
            }
            _configureActions.Clear();

            if (service is VariableService concreteService)
            {
                foreach (var variableProvider in setup.Providers)
                {
                    concreteService.RegisterProvider(variableProvider);
                }

                if (setup.AddEnvironmentVariablesWithoutSetup)
                {
                    concreteService.AddEnvironmentVariables();
                }
            }

            _isLocked = true;
            return service;
        });

        return services;
    }

    internal static IServiceCollection AddSlimVariableServices(this IServiceCollection services, IConfigurationRoot config, bool ignoreLock)
    {
        services
            .AddSingleton<IConfiguration>(config)
            .AddSingleton<IVariableService>(provider =>
            {
                if (!_isLocked || ignoreLock)
                {
                    var service = ActivatorUtilities.CreateInstance<VariableService>(provider);

                    // Add Environment Setup (it will always needed)
                    service.RegisterSetup<EnvironmentSetup>();

                    return service;
                }
                else
                {
                    throw new SdkException("VariableService is locked and cannot be used over SlimHost");
                }
            });

        return services;
    }
}
