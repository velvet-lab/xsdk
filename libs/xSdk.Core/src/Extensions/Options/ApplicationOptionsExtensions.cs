using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using static xSdk.Extensions.Options.ApplicationOptions.Definitions;

namespace xSdk.Extensions.Options;

public static class ApplicationOptionsExtensions
{
    public static IServiceCollection RegisterApplicationOptions(this IServiceCollection services, ApplicationOptions options)
    {
        services
            .AddOptions<ApplicationOptions>()
            .Configure(appOptions =>
            {
                appOptions.Name = options.Name ?? ApplicationOptions.Definitions.AppName.DefaultValue;
                appOptions.Company = options.Company ?? ApplicationOptions.Definitions.AppCompany.DefaultValue;
                appOptions.Prefix = options.Prefix ?? ApplicationOptions.Definitions.AppPrefix.DefaultValue;

                appOptions.Description = options.Description;
                appOptions.AppVersion = options.AppVersion;
                
                var validator = new ApplicationOptionsValidator();
                validator.ValidateAndThrow(appOptions);
            });

        return services;
    }
}
