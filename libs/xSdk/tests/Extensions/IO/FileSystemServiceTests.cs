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

namespace xSdk.Extensions.IO;

public class FileSystemServiceTests : IClassFixture<TestHostFixture>
{
    private readonly IFileSystemService _service;

    public FileSystemServiceTests(TestHostFixture fixture)
    {
        _service = fixture
            .BuildHost()
            .Services.GetRequiredService<IFileSystemService>();
    }

    [Theory]
    [InlineData(FileSystemContext.Machine)]
    [InlineData(FileSystemContext.User)]
    [InlineData(FileSystemContext.Local)]
    [InlineData(FileSystemContext.None)]
    public void RequestMachineFileSystem(FileSystemContext context)
    {
        Assert.NotNull(_service);

        IFileSystemResult fs = _service.RequestFileSystem(context);
        Assert.NotNull(fs);
    }
}
