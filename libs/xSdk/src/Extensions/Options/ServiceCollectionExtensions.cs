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
        => services.RegisterOptions<TOptions>(null, options => { });

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
        var assembly = typeof(TOptions).Assembly;
        var validatorTypes = assembly.GetTypes()
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
