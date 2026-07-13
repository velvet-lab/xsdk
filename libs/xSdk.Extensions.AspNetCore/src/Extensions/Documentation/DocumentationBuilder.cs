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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using xSdk.Extensions.Builder;

namespace xSdk.Extensions.Documentation;

public sealed class DocumentationBuilder : BuilderBase
{
    internal DocumentationOptions Options => Services.GetRequiredService<IOptions<DocumentationOptions>>().Value;

    internal Func<ApiVersionDescription, OpenApiInfo>? CreateApiInfoAction
    {
        get => field ?? (_ => CreateApiInfo(_));
        set;
    }

    private OpenApiInfo CreateApiInfo(ApiVersionDescription description)
        => new()
        {
            Title = "SDK API Documentation",
            Version = "v1",
            Description =
                "Default API Documentation for xSDK. Convert replace the default Documentation use the IDocumentationPluginBuilder Interface while the plugin will enabled.",
            License = new OpenApiLicense { Name = "MIT" },
        };
}
