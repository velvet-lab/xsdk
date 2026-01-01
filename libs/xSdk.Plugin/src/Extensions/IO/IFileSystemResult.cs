using Zio;

namespace xSdk.Extensions.IO
{
    public interface IFileSystemResult
    {
        public IFileSystem App { get; }

        public IFileSystem Data { get; }
    }
}
