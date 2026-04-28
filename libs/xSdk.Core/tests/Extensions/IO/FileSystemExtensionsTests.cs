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

using Zio;
using Zio.FileSystems;

namespace xSdk.Extensions.IO;

public class FileSystemExtensionsTests
{
    [Fact]
    public void GetFullPath_MemoryFileSystem_ReturnsRootPath()
    {
        using var fs = new MemoryFileSystem();

        var path = fs.GetFullPath();

        Assert.NotNull(path);
        Assert.NotEmpty(path);
    }

    [Fact]
    public void GetFullPath_WithStringPath_ReturnsAbsolutePath()
    {
        using var fs = new MemoryFileSystem();

        var path = fs.GetFullPath("/some/directory");

        Assert.NotNull(path);
        Assert.NotEmpty(path);
    }

    [Fact]
    public void GetFullPath_WithUPath_ReturnsAbsolutePath()
    {
        using var fs = new MemoryFileSystem();
        var upath = new UPath("/some/directory");

        var path = fs.GetFullPath(upath);

        Assert.NotNull(path);
        Assert.NotEmpty(path);
    }
}
