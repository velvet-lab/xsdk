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
using MongoDB.Driver;
using Testcontainers.MongoDb;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private MongoDbContainer? _mongoDbContainer = null;

    protected override void Initialize()
    {
        ConfigureBuilder(hostBuilder => hostBuilder
            .AddDatalayer(builder => builder
                .UseEntityFramework<TestDbContext>()
                .MapRepository<ITestRepository, TestRepository>()))

            .ConfigureServices(services => services
                    // Add DbContext Factory
                    .AddDbContextFactory<TestDbContext>(options =>
                    {
                        // Use TestContainers for UnitTests
                        var imageName = GetEnvironmentVariable("MONGODB_IMAGE_NAME");
                        _mongoDbContainer = new MongoDbBuilder(imageName)
                            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(27017)))
                            .WithAutoRemove(true)
                            .Build();

                        _mongoDbContainer.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        var connectionString = _mongoDbContainer.GetConnectionString();

                        var client = new MongoClient(connectionString);
                        options.UseMongoDB(client, Globals.DatabaseName);
                    }));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                _mongoDbContainer?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // Nothing to tell
            }
        }
        base.Dispose(disposing);
    }
}
