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

using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using NLog;
using Testcontainers.MongoDb;
using xSdk.Data;
using xSdk.Demos.Data;
using xSdk.Demos.Hosting;

const string APP_NAME = "datalayer-mongodb";
const string APP_COMPANY = "xdemos";
const string APP_PREFIX = "dn";

Logger logger = null;

var mongoDbContainer = new MongoDbBuilder()
    .WithImage("mongo:8.0")
    .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(27017)))
    .WithPortBinding(27018)
    .WithUsername("host")
    .WithPassword("password123")
    .WithReuse(true)
    .Build();

await mongoDbContainer.StartAsync();

var host = xSdk
    .Hosting.Host.CreateBuilder(args, APP_NAME, APP_COMPANY, APP_PREFIX)
    .ConfigureServices(
        (context, services) =>
        {
            services
                // Add DbContext Factory
                .AddDbContextFactory<SampleDbContext>(options =>
                {
                    var connectionString = mongoDbContainer.GetConnectionString();
                    logger?.Info("MongoDB ConnectionString: {ConnectionString}", connectionString);

                    var client = new MongoClient(connectionString);

                    // Use InMemory Database
                    options.UseMongoDB(client, "MyDataStore");
                })
                // Sample for NoSql Datalayer
                .AddDatalayer(builder =>
                {
                    builder
                        .UseEntityFramework<SampleDbContext>(
                            "MySampleDatalayer",
                            config =>
                            {
                                config.TransactionsEnabled = false;
                            }
                        )
                        // Add Repositories to the Layer
                        .MapRepository<ISampleRepository, SampleRepository>();
                })
                .AddHostedService<MyDataHost>();
        }
    )
    .Build();

logger = LogManager.GetCurrentClassLogger();
logger.Info("Starting {AppName}", APP_NAME);

await host.RunAsync();
