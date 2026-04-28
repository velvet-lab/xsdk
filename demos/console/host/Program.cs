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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;
using xSdk.Plugins.Commands;

const string APP_NAME = "host";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "ho";

var host = xSdk
    .Hosting.Host
        .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)

        // Enable Commands with default configuration
        .EnableCommands()

        // or with custom configuration
        // .EnableCommands<CommandsPluginBuilder>()
        .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.LogInformation("Starting {AppName}", APP_NAME);

return await host.RunConsoleAsync(args);
