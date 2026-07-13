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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using xSdk.Demos;
using xSdk.Demos.Builders;
using xSdk.Extensions.Logging;
using xSdk.Hosting;
using xSdk.Plugins.Telemetry;

const string APP_NAME = "AutomationHub";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "ah";

var host = xSdk.Hosting.Host
    .CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .EnableTelemetry(MyTelemetryBuilder.ConfigureBuilder, options =>
    {
        options.LoggingEnabled = true;
        options.TracingEnabled = true;
        options.MetricsEnabled = true;
    })
    .ConfigureServices(services => services.AddSingleton<LocalService>())
    // Ein eigener Host der benutzt werden soll
    .AddHost<MyHost>()
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.LogInformation("Starting {AppName}", APP_NAME);

await host.RunAsync();
