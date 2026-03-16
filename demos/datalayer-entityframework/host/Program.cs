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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Data;
using xSdk.Demos;
using xSdk.Demos.Data;
using xSdk.Demos.Hosting;

const string APP_NAME = "datalayer-entityframework";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "de";

var host = xSdk
    .Hosting.Host.CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .ConfigureServices(
        (context, services) =>
        {
            services
                // Add DbContext Factory
                .AddDbContextFactory<SampleDbContext>(options =>
                {
                    // Use InMemory Database
                    options.UseInMemoryDatabase("MySampleDatabase").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                })
                // Sample for Entity Datalayer
                .AddDatalayer(builder =>
                {
                    builder
                        // Enable Entityframework
                        .UseEntityFramework<SampleDbContext>(
                            DbProviderNames.First,
                            config =>
                            {
                                config.TransactionsEnabled = false;
                            }
                        )
                        // Add Repositories to the Layer
                        .MapRepository<ISampleRepository, SampleRepository>(DbProviderNames.First);

                    builder
                        // Enable Entityframework for second DbContext
                        .UseEntityFramework<SecondDbContext>(
                            DbProviderNames.Second,
                            config =>
                            {
                                config.TransactionsEnabled = false;
                            }
                        )
                        // Add Repositories to the Layer
                        .MapRepository<ISecondSampleRepository, SecondSampleRepository>(DbProviderNames.Second);
                })
                .AddHostedService<MyDataHost>();
        }
    )
    .Build();

var logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
