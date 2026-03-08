using Asp.Versioning.ApiExplorer;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Plugins.Authentication;
using xSdk.Shared;

namespace xSdk.Plugins.Documentation;

internal sealed class DocumentationPlugin : WebHostPluginBase
{
    private static readonly OpenApiInfo DefaultApiInfo = new OpenApiInfo
    {
        Title = "SDK API Documentation",
        Version = "v1",
        Description =
            "Default API Documentation for xSDK. Convert replace the default Documentation use the IDocumentationPluginBuilder Interface while the plugin will enabled.",
        License = new OpenApiLicense { Name = "MIT" },
    };


    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        // Hack: Retrieve currently configured ApiVersions from previously loaded ApiVersionProvider
        // Convert do this, it is neccessary to build the service provider
        var descriptionProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();

        var authPlugin = SlimHost.Instance.PluginSystem.GetPlugin<AuthenticationPlugin>();
        var docPluginBuilder = SlimHost.Instance.PluginSystem.GetPlugin<IDocumentationPluginBuilder>();

        services
            .AddSwaggerGen(options =>
            {
                Logger.Info("Configure Swagger Document Generator");

                foreach (var description in descriptionProvider.ApiVersionDescriptions)
                {
                    var apiInfo = docPluginBuilder?.CreateApiInfo(description);
                    if (apiInfo == null)
                    {
                        apiInfo = DefaultApiInfo;
                    }
                    options.SwaggerDoc(description.GroupName, apiInfo);
                }

                options.EnableAnnotations();
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
                options.ExampleFilters();

                Logger.Trace("Add Docu Infos from current Assembly");
                LoadXmlDocumentations(options);

                docPluginBuilder?.ConfigureSwagger(options);
            });

        Logger.Debug("Enable Swagger Example Generator");
        var assemblies = AssemblyCollector.Collect();
        services.AddSwaggerExamplesFromAssemblies(assemblies.ToArray());

        Logger.Debug("Enable FluentValidation Rules to Swagger");
        services.AddFluentValidationRulesToSwagger();
    }

    public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
    {
        var pluginSvc = app.ApplicationServices.GetRequiredService<IPluginService>();
        var descriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        var plugins = pluginSvc.GetPlugins();
        var authPlugin = pluginSvc.GetPlugin<AuthenticationPlugin>();


        // Enable middleware to serve generated Swagger as a JSON endpoint.
        // see also https://cpratt.co/customizing-swagger-ui-in-asp-net-core/
        app
            .UseSwagger(setup =>
            {
                // setup.SerializeAsV2 = true;
            })

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            .UseSwaggerUI(setup =>
            {
                var docSetup = SlimHost.Instance.VariableSystem.GetSetup<DocumentationSetup>();

                setup.RoutePrefix = docSetup.RoutePrefix;
                foreach (var description in descriptionProvider.ApiVersionDescriptions)
                {
                    setup.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }

                setup.EnableValidator();

                SlimHost.Instance.PluginSystem.Invoke<IDocumentationPluginBuilder>(x => x.ConfigureSwaggerUi(setup));
            });
    }

    private void LoadXmlDocumentations(SwaggerGenOptions setup)
    {
        // Set the comments path for the Swagger JSON and UI.
        var executionFolder = FileSystemHelper.GetExecutingFolder();
        var xmlFiles = Directory.GetFiles(executionFolder, "*.xml");
        foreach (var xmlFile in xmlFiles)
        {
            FileInfo fileInfo = new FileInfo(xmlFile);
            Logger.Debug("Load Documenation from file '{0}'", fileInfo.Name);
            setup.IncludeXmlComments(xmlFile);
        }
    }
}
