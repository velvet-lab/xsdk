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
using Microsoft.OpenApi;
using xSdk.Extensions.Documentation;
using xSdk.Extensions.Plugin;

namespace xSdk.Demos.Builders;

public class DocumentationPluginBuilder : PluginBuilder, IDocumentationPluginBuilder
{
    public OpenApiInfo CreateApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Sample API",
            Version = description.ApiVersion.ToString(),
            Description = "Sample API Documentation.",
            License = new OpenApiLicense { Name = "MIT" },
        };

        if (description.GroupName == "v2")
        {
            info.Title = "Sample API Test";
        }

        if (description.GroupName == "v3")
        {
            info.Title = "Sample API with HATEOAS Links";
        }

        if (description.IsDeprecated)
        {
            info.Description += " [This API version has been deprecated]";
        }

        return info;
    }
}
