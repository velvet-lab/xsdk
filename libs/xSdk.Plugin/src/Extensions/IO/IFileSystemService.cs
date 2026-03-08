namespace xSdk.Extensions.IO;

public interface IFileSystemService
{
    IFileSystemResult RequestFileSystem(FileSystemContext context = FileSystemContext.None) => RequestFileSystemAsync(context).GetAwaiter().GetResult();

    Task<IFileSystemResult> RequestFileSystemAsync(FileSystemContext context = FileSystemContext.None, CancellationToken token = default);

    IFileSystemResult Local { get; }

    IFileSystemResult User { get; }

    IFileSystemResult Machine { get; }
}
