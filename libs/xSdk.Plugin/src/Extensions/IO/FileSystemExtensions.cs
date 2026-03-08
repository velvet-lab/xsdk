using Zio;

namespace xSdk.Extensions.IO;

[CLSCompliant(false)]
public static class FileSystemExtensions
{
    public static string GetFullPath(this IFileSystem fileSystem) => fileSystem.GetFullPath("/");

    public static string GetFullPath(this IFileSystem fileSystem, UPath path)
    {
        var (fs, fullPath) = fileSystem.ResolvePath(path);
        return fs.ConvertPathToInternal(fullPath);
    }

    public static string GetFullPath(this IFileSystem fileSystem, string path)
    {
        var (fs, fullPath) = fileSystem.ResolvePath(path);
        return fs.ConvertPathToInternal(fullPath);
    }
}
