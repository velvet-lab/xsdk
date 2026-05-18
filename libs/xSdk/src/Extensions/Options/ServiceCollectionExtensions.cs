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

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services)
        where TOptions : class, IVariableSetup
        => services.RegisterOptions<TOptions>(options => { });

    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services, string? name)
        where TOptions : class, IVariableSetup
        => services.RegisterOptions<TOptions>(name, options => { });

    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services, Action<TOptions>? configure)
        where TOptions : class, IVariableSetup
        => services.RegisterOptions(null, configure);

    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services, string? name, Action<TOptions>? configure)
        where TOptions : class, IVariableSetup
    {
        services
            .AddOptions<TOptions>(name)
            .Configure<IVariableService, IValidator<TOptions>>((options, variableService, validator) =>
            {
                variableService.ParseForVariables(options);

                if (options is VariableSetup variableSetup)
                {
                    variableSetup.Initialize(variableService);
                }

                configure?.Invoke(options);
                validator.ValidateAndThrow(options);
            });

        services.SearchAndRegisterValidators<TOptions>();

        return services;
    }

    private static void SearchAndRegisterValidators<TOptions>(this IServiceCollection services)
        where TOptions : class, IVariableSetup
    {
        Assembly assembly = typeof(TOptions).Assembly;
        IEnumerable<Type> validatorTypes = assembly.GetTypes()
            .Where(type => typeof(AbstractValidator<TOptions>).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface);

        if (validatorTypes.Any())
        {
            foreach (Type validatorType in validatorTypes)
            {
                services.TryAddSingleton(typeof(IValidator<TOptions>), validatorType);
            }
        }
        else
        {
            services.RegisterDefaultValidator<TOptions>();
        }
    }

    private static void RegisterDefaultValidator<TOptions>(this IServiceCollection services)
        where TOptions : class, IVariableSetup
    {
        services.TryAddSingleton<IValidator<TOptions>, DefaultOptionsValidator<TOptions>>();
    }
}
