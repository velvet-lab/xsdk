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
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class CrudDatabaseFixture : DatabaseHostFixture
{
    private readonly string _dbName = $"CrudTestDb_{Guid.NewGuid():N}";

    protected override void Initialize()
    {
        ConfigureBuilder(builder =>
        {
            builder
                .AddDatalayer(builder =>
                 {
                     builder
                         .UseEntityFramework<TestDbContext>(Globals.DatalayerName)
                         .MapRepository<ITestRepository, TestRepository>();
                 });
        });

        ConfigureServices(services =>
        {
            services
                .AddDbContextFactory<TestDbContext>(options =>
                {
                    options
                        .UseInMemoryDatabase(_dbName)
                        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
        });
    }
}
