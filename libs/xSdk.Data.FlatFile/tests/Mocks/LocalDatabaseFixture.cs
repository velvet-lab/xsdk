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
using xSdk.Hosting;

namespace xSdk.Data.Mocks;

public class LocalDatabaseFixture : DatabaseHostFixture
{
    private readonly string _tempFolder;

    public LocalDatabaseFixture()
    {
        _tempFolder = Path.Combine(Path.GetTempPath(), "xsdk-flatfile-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempFolder);
    }

    protected override void Initialize()
    {
        ConfigureServices(services =>
        {
            services.AddDatalayer(builder =>
            {
                builder
                    .UseFlatFile(
                        Globals.DatalayerName,
                        config =>
                        {
                            config.FilePath = _tempFolder;
                        }
                    )
                    .MapRepository<ITestRepository, TestRepository>(Globals.DatalayerName);
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            try
            {
                if (Directory.Exists(_tempFolder))
                    Directory.Delete(_tempFolder, recursive: true);
            }
            catch
            {
                // cleanup is best-effort
            }
        }
    }
}
