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

namespace xSdk.Extensions.IO;

public class FileSystemHelperTests
{
    [Fact]
    public void GetExecutingFolder_ReturnsNonEmptyString()
    {
        string folder = FileSystemHelper.GetExecutingFolder();

        Assert.NotNull(folder);
        Assert.NotEmpty(folder);
    }

    [Fact]
    public void NormalizePath_PathWithoutLeadingSlash_AddsSlash()
    {
        string? result = FileSystemHelper.NormalizePath("some/path");

        Assert.StartsWith("/", result);
        Assert.Equal("/some/path", result);
    }

    [Fact]
    public void NormalizePath_PathWithLeadingSlash_DoesNotAddSlash()
    {
        string? result = FileSystemHelper.NormalizePath("/some/path");

        Assert.Equal("/some/path", result);
    }

    [Fact]
    public void NormalizePath_EmptyString_ReturnsEmpty()
    {
        string? result = FileSystemHelper.NormalizePath(string.Empty);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void NormalizePath_NullString_ReturnsNull()
    {
        Assert.Null(FileSystemHelper.NormalizePath(null));
    }

    [Fact]
    public void GetFullPath_ValidPath_ReturnsUPath()
    {
        string? folder = FileSystemHelper.GetExecutingFolder();

        UPath result = FileSystemHelper.GetFullPath(folder);

        Assert.NotNull(result.ToString());
    }

    [Fact]
    public void SearchGitRoot_FromCurrentDirectory_FindsGitRoot()
    {
        string? currentDir = Environment.CurrentDirectory;

        string? result = FileSystemHelper.SearchGitRoot(currentDir);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void SearchGitRoot_WithEmptyString_ReturnsFallback()
    {
        string? result = FileSystemHelper.SearchGitRoot(string.Empty);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void SearchGitRoot_WithNullString_ReturnsFallback()
    {
        string? result = FileSystemHelper.SearchGitRoot(null);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CreateSpecificDataFolder_CreatesAndReturnsPath()
    {
        using var fs = new Zio.FileSystems.MemoryFileSystem();
        fs.CreateDirectory("/root");

        string? result = FileSystemHelper.CreateSpecificDataFolder(fs, "/root/data");

        Assert.NotNull(result);
        Assert.True(fs.DirectoryExists("/root/data"));
    }

    [Fact]
    public void CreateSpecificDataFolder_ExistingDirectory_ReturnsPath()
    {
        using var fs = new Zio.FileSystems.MemoryFileSystem();
        fs.CreateDirectory("/root/existing");

        string? result = FileSystemHelper.CreateSpecificDataFolder(fs, "/root/existing");

        Assert.NotNull(result);
    }

    [Fact]
    public void IsDirectoryWritable_ExistingTempDir_ReturnsTrue()
    {
        string tempDir = Path.GetTempPath();

        bool result = FileSystemHelper.IsDirectoryWritable(tempDir);

        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryWritable_NonExistingDir_ReturnsFalse()
    {
        string fakePath = Path.Combine(Path.GetTempPath(), "nonexistent-" + Guid.NewGuid());

        bool result = FileSystemHelper.IsDirectoryWritable(fakePath);

        Assert.False(result);
    }

    [Fact]
    public void IsDirectoryWritable_DirectoryInfo_ExistingDir_ReturnsTrue()
    {
        var dir = new DirectoryInfo(Path.GetTempPath());

        bool result = FileSystemHelper.IsDirectoryWritable(dir);

        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryReadable_ExistingDir_ReturnsTrue()
    {
        string tempDir = Path.GetTempPath();

        bool result = FileSystemHelper.IsDirectoryReadable(tempDir);

        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryReadable_NonExistingDir_ReturnsFalse()
    {
        string fakePath = Path.Combine(Path.GetTempPath(), "nonexistent-" + Guid.NewGuid());

        bool result = FileSystemHelper.IsDirectoryReadable(fakePath);

        Assert.False(result);
    }

    [Fact]
    public void IsDirectoryReadable_DirectoryInfo_ExistingDir_ReturnsTrue()
    {
        var dir = new DirectoryInfo(Path.GetTempPath());

        bool result = FileSystemHelper.IsDirectoryReadable(dir);

        Assert.True(result);
    }
}
