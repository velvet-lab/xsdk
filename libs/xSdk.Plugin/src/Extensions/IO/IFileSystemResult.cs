using Zio;

namespace xSdk.Extensions.IO;

public interface IFileSystemResult
{
    IFileSystem App { get; }

    IFileSystem Data { get; }
}
