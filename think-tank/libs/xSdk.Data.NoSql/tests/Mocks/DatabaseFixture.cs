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
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using xSdk.Extensions.IO;
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class DatabaseFixture : DatabaseHostFixture
{
    private readonly IContainer? _container = null;

    protected override void Initialize()
    {
        ConfigureBuilder(hostBuilder =>
        {
            hostBuilder
                .AddDatalayer(builder =>
                {
                    var currentFolder = Path.Combine(Path.GetTempPath(), "data", Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(currentFolder))
                    {
                        Directory.CreateDirectory(currentFolder);
                    }

                    builder
                        // Enable FlatFile
                        .UseNoSql(config =>
                            {
                                config.FileName = Path.Combine(currentFolder, $"{Globals.DatabaseName}.store");
                            }
                        )
                        .MapRepository<ITestRepository, TestRepository>();
                });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                _container?.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch
            {
                // Nothing to tell
            }
        }
        base.Dispose(disposing);
    }
}
