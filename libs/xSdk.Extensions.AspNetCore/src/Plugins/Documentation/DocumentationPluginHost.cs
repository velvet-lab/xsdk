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

using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using xSdk.Extensions.Documentation;
using xSdk.Hosting;

namespace xSdk.Plugins.Documentation;

public sealed class DocumentationPluginHost(IOptions<DocumentationOptions> options) : WebPluginHost
{
    private static readonly OpenApiInfo _defaultApiInfo = new OpenApiInfo
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

        var docPluginBuilder = GetBuilder<IDocumentationPluginBuilder>();

        var documentationOptions = options.Value;
        if (documentationOptions.Enabled)
        {
            foreach (var description in descriptionProvider.ApiVersionDescriptions)
            {
                services.AddOpenApi(description.GroupName, options =>
                {
                    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
                    options.ShouldInclude = (current) => current.GroupName == description.GroupName;

                    options.AddDocumentTransformer((document, context, cancellationToken) =>
                    {
                        var apiInfo = docPluginBuilder.CreateApiInfo(description);
                        if (apiInfo == null)
                        {
                            apiInfo = _defaultApiInfo;
                        }
                        document.Info = apiInfo;
                        return Task.CompletedTask;
                    });

                    //options.AddOperationTransformer((operation, context, cancellationToken) =>                
                    //{

                    //    return Task.CompletedTask;
                    //});

                    //options.AddSchemaTransformer((schema, context, cancellationToken) =>
                    //{                    
                    //    return Task.CompletedTask;
                    //});
                });
            }
        }
    }

    public override void ConfigureEndpoint(IEndpointRouteBuilder builder)
    {
        var documentationOptions = options.Value;
        if (documentationOptions.Enabled && !string.IsNullOrEmpty(documentationOptions.DocumentPattern))
        {
            builder.MapOpenApi(documentationOptions.DocumentPattern);
        }
    }
}
