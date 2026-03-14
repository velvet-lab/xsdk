using xSdk.Extensions.IO;

namespace xSdk.Plugin.Tests.Extensions.IO;

public class FileSystemHelperTests
{
    [Fact]
    public void GetExecutingFolder_ReturnsNonEmptyString()
    {
        var folder = FileSystemHelper.GetExecutingFolder();

        Assert.NotNull(folder);
        Assert.NotEmpty(folder);
    }

    [Fact]
    public void NormalizePath_PathWithoutLeadingSlash_AddsSlash()
    {
        var result = FileSystemHelper.NormalizePath("some/path");

        Assert.StartsWith("/", result);
        Assert.Equal("/some/path", result);
    }

    [Fact]
    public void NormalizePath_PathWithLeadingSlash_DoesNotAddSlash()
    {
        var result = FileSystemHelper.NormalizePath("/some/path");

        Assert.Equal("/some/path", result);
    }

    [Fact]
    public void NormalizePath_EmptyString_ReturnsEmpty()
    {
        var result = FileSystemHelper.NormalizePath(string.Empty);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void NormalizePath_NullString_ReturnsNull()
    {
        var result = FileSystemHelper.NormalizePath(null);

        Assert.Null(result);
    }

    [Fact]
    public void GetFullPath_ValidPath_ReturnsUPath()
    {
        var folder = FileSystemHelper.GetExecutingFolder();

        var result = FileSystemHelper.GetFullPath(folder);

        Assert.NotNull(result.ToString());
    }

    [Fact]
    public void SearchGitRoot_FromCurrentDirectory_FindsGitRoot()
    {
        var currentDir = Environment.CurrentDirectory;

        var result = FileSystemHelper.SearchGitRoot(currentDir);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void SearchGitRoot_WithEmptyString_ReturnsFallback()
    {
        var result = FileSystemHelper.SearchGitRoot(string.Empty);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void SearchGitRoot_WithNullString_ReturnsFallback()
    {
        var result = FileSystemHelper.SearchGitRoot(null);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}
