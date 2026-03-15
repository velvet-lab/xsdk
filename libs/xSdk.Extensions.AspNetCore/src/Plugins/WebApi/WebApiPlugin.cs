using System.Diagnostics;
using Asp.Versioning;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Data;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Plugins.WebApi;

public class WebApiPlugin : WebHostPluginBase
{
    protected override int Order => 50;

    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        Logger.Trace("Load Setups for Web Host Builder");
        services
            // Add Context Accessor
            .AddHttpContextAccessor()
            .AddProblemDetails(_ =>
            {
                Logger.Debug("Configure Problem Details");
                var currentStage = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>().Stage;

                _.IncludeExceptionDetails = (ctx, ex) =>
                {
                    if (Debugger.IsAttached || currentStage == Stage.Development || currentStage == Stage.Integration)
                        return true;

                    return false;
                };
                _.ShouldLogUnhandledException = (ctx, ex, details) =>
                {
                    return true;
                };
            })
            // Add Routing
            .AddRouting(_ =>
            {
                Logger.Debug("Configure Routing");
                _.LowercaseUrls = true;
                _.LowercaseQueryStrings = true;
                _.SuppressCheckForUnhandledSecurityMetadata = false;
            });

        var mvcBuilder = services
            .AddControllers(_ =>
            {
                Logger.Debug("Configure Mvc");
                _.InputFormatters.Add(new PlainTextFormatter());
                _.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

                SlimHost.Instance.PluginSystem.Invoke<IWebApiPluginBuilder>(x => x.ConfigureMvc(_));
            })
            .AddJsonOptions(_ =>
            {
                Logger.Debug("Configure Json");
                _.JsonSerializerOptions.ConfigureSerializerOptions();
            });

        // Enabled Versioning
        services
            .AddApiVersioning(_ =>
            {
                Logger.Debug("Enabled Api Versioning");

                // Add the headers "api-supported-versions" and "api-deprecated-versions"
                // This is better for discoverability
                _.ReportApiVersions = true;

                // AssumeDefaultVersionWhenUnspecified should only be enabled when supporting legacy services that did not previously
                // support API versioning. Forcing existing clients to specify an explicit API version for an
                // existing service introduces a breaking change. Conceptually, clients in this situation are
                // bound to some API version of a service, but they don't know what it is and never explicit request it.
                _.AssumeDefaultVersionWhenUnspecified = true;
                _.DefaultApiVersion = new ApiVersion(1, 0);

                // Defines how an API version is read from the current HTTP request
                _.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-API-Version"),
                    new MediaTypeApiVersionReader("version")
                );
            })
            .AddMvc()
            .AddApiExplorer(_ =>
            {
                _.GroupNameFormat = "'v'V";
                _.SubstituteApiVersionInUrl = true;
            });

        Logger.Debug("Enabled Endpoints for API Explorer");
        services.AddEndpointsApiExplorer();

        var assemblies = AssemblyCollector.Collect();

        Logger.Debug("Add Fluent Validation");
        services.AddValidatorsFromAssemblies(assemblies);

        Logger.Debug("Add Application Parts");
        foreach (var assembly in assemblies)
        {
            mvcBuilder.AddApplicationPart(assembly);
        }
    }

    public override void ConfigureDefaults(WebHostBuilderContext context, IApplicationBuilder app)
    {
        Logger.Debug("Load Environtment Setup");
        var envSetup = app.ApplicationServices.GetRequiredService<IVariableService>().GetSetup<IEnvironmentSetup>();

        if (envSetup != null && envSetup.Stage == Stage.Development)
        {
            app.UseDeveloperExceptionPage();
        }

        Logger.Debug("Enabled HTTPS Redirection");
        app.UseHttpsRedirection();

        app.UseRouting();
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        app
            .UseStatusCodePages()
            .UseProblemDetails();
    }

    public override void ConfigureEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapControllers();

    }
}
