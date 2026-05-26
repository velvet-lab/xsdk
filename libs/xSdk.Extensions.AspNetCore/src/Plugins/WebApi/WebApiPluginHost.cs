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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Extensions.WebApi;
using xSdk.Hosting;
using xSdk.Shared;
using xSdk.Tools;

namespace xSdk.Plugins.WebApi;

[ExcludeFromCodeCoverage(Justification = "ASP.NET Core MVC/WebApi pipeline configuration – requires a running web host.")]
internal sealed class WebApiPluginHost(IOptions<EnvironmentOptions> environmentOptions, IPluginHostCollection pluginHostCollection) : WebPluginHost
{
    public override int Order => 50;

    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        Logger.LogTrace("Load Setups for Web Host Builder");
        services
            // Add Context Accessor
            .AddHttpContextAccessor()
            .AddProblemDetails(_ =>
            {
                Logger.LogDebug("Configure Problem Details");
                var currentStage = environmentOptions.Value.Stage;

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
                Logger.LogDebug("Configure Routing");
                _.LowercaseUrls = true;
                _.LowercaseQueryStrings = true;
                _.SuppressCheckForUnhandledSecurityMetadata = false;
            });

        var mvcBuilder = services
            .AddControllers(_ =>
            {
                Logger.LogDebug("Configure Mvc");
                _.InputFormatters.Add(new PlainTextFormatter());
                _.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;

                InvokeBuilders<IWebApiPluginBuilder>(plugin => plugin.ConfigureMvc(_));

            })
            .AddJsonOptions(_ =>
            {
                Logger.LogDebug("Configure Json");
                _.JsonSerializerOptions.ConfigureSerializerOptions();
            });

        // Enabled Versioning
        services
            .AddApiVersioning(_ =>
            {
                Logger.LogDebug("Enabled Api Versioning");

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

        Logger.LogDebug("Enabled Endpoints for API Explorer");
        services.AddEndpointsApiExplorer();

        var assemblies = AssemblyCollector.Collect(pluginHostCollection);

        Logger.LogDebug("Add Fluent Validation");
        services.AddValidatorsFromAssemblies(assemblies);

        Logger.LogDebug("Add Application Parts");
        foreach (var assembly in assemblies)
        {
            mvcBuilder.AddApplicationPart(assembly);
        }
    }

    public override void ConfigureDefaults(WebHostBuilderContext context, IApplicationBuilder app)
    {
        Logger.LogDebug("Load Environtment Setup");
        if (environmentOptions.Value.Stage == Stage.Development)
        {
            app.UseDeveloperExceptionPage();
        }

        Logger.LogDebug("Enabled HTTPS Redirection");
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
