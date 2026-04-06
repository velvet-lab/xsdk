using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services)
        where TOptions : VariableSetup
        => services.RegisterOptions<TOptions>(options => { });

    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services, Action<TOptions> configure)
        where TOptions : VariableSetup
    {
        services
            .AddOptions<TOptions>()
            .Configure<IVariableService, IValidator<TOptions>>((options, variableService, validator) =>
            {
                variableService.ParseForVariables<TOptions>(options);
                options.Initialize(variableService);

                configure?.Invoke(options);

                validator.ValidateAndThrow(options);                
            });


        services.SearchAndRegisterValidators<TOptions>();

        return services;
    }

    private static void SearchAndRegisterValidators<TOptions>(this IServiceCollection services)
        where TOptions : VariableSetup
    {
        var assembly = typeof(TOptions).Assembly;
        assembly.GetTypes()
            .Where(type => typeof(AbstractValidator<TOptions>).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .ToList()
            .ForEach(type => services.TryAddSingleton(typeof(IValidator<TOptions>), type));
    }
}
