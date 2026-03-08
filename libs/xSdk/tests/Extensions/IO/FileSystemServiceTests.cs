using xSdk.Hosting;

namespace xSdk.Extensions.IO;

public class FileSystemServiceTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    [Theory]
    [InlineData(FileSystemContext.Machine)]
    [InlineData(FileSystemContext.User)]
    [InlineData(FileSystemContext.Local)]
    [InlineData(FileSystemContext.None)]
    public void RequestMachineFileSystem(FileSystemContext context)
    {
        var service = fixture
            .ConfigureServices(services => services.AddFileServices())
            .GetService<IFileSystemService>();

        Assert.NotNull(service);

        var fs = service.RequestFileSystem(context);
        Assert.NotNull(fs);
    }
}
