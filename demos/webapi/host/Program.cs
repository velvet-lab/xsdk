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

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using xSdk.Demos.Builders;
using xSdk.Extensions.Documentation;
using xSdk.Extensions.Logging;
using xSdk.Plugins.Authentication;
using xSdk.Plugins.Compression;
using xSdk.Plugins.DataProtection;
using xSdk.Plugins.Documentation;
using xSdk.Plugins.Links;
using xSdk.Plugins.WebApi;
using xSdk.Plugins.WebSecurity;

[assembly: ApiController]
[assembly: ApiConventionType(typeof(DefaultApiConventions))]

const string APP_NAME = "webapi";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "webapi";

IHost host = xSdk.Hosting.WebHost
    .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .EnableWebApi()
    .EnableDocumentation(builder =>
    {
        builder
            .WithApiInfo(description =>
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
            });
    })
    .EnableWebSecurity()
    .EnableAuthentication<MyAuthenticationBuilder>()
    .EnableCompression()
    .EnableDataProtection()
    .EnableLinks<MyLinksBuilder>()
    .Build();

ILogger logger = LogManager.GetCurrentClassLogger();
logger.LogInformation("Starting {AppName}", APP_NAME);

await host.RunAsync();
