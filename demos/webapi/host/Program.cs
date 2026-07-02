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
using xSdk.Demos.Builders;
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
    .EnableDocumentation<DocumentationPluginBuilder>()
    .EnableWebSecurity()
    .EnableAuthentication<AuthenticationPluginBuilder>()
    .EnableCompression()
    .EnableDataProtection()
    .EnableLinks<LinksPluginBuilder>()
    .Build();

ILogger logger = LogManager.GetCurrentClassLogger();
logger.LogInformation("Starting {AppName}", APP_NAME);

await host.RunAsync();
