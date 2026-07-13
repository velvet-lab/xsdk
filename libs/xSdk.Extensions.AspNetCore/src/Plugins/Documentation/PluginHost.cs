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

internal sealed class PluginHost(DocumentationBuilder builder, IOptions<DocumentationOptions> options) : WebPluginHost
{

    public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        // Hack: Retrieve currently configured ApiVersions from previously loaded ApiVersionProvider
        // Convert do this, it is neccessary to build the service provider
        IApiVersionDescriptionProvider descriptionProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();

        DocumentationOptions documentationOptions = options.Value;
        if (documentationOptions.Enabled)
        {
            foreach (ApiVersionDescription description in descriptionProvider.ApiVersionDescriptions)
            {
                services.AddOpenApi(description.GroupName, options =>
                {
                    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
                    options.ShouldInclude = (current) => current.GroupName == description.GroupName;

                    options.AddDocumentTransformer((document, context, cancellationToken) =>
                    {
                        OpenApiInfo? apiInfo = default;

                        apiInfo = builder.CreateApiInfoAction(description);
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
        DocumentationOptions documentationOptions = options.Value;
        if (documentationOptions.Enabled && !string.IsNullOrEmpty(documentationOptions.DocumentPattern))
        {
            builder.MapOpenApi(documentationOptions.DocumentPattern);
        }
    }
}
