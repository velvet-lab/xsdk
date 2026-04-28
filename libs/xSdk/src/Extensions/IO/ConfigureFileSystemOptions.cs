using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.IO;

internal sealed class ConfigureFileSystemOptions(IOptions<ApplicationOptions> appOptions, IOptions<EnvironmentOptions> envOptions) : IConfigureOptions<FileSystemOptions>
{
    private readonly ApplicationOptions _appOptions = appOptions.Value;
    private readonly EnvironmentOptions _envOptions = envOptions.Value;

    public void Configure(FileSystemOptions options)
    {
        options.ApplicationName = _appOptions?.Name;
        options.Company = _appOptions?.Company;
        options.ContentRoot = _envOptions?.ContentRoot;
    }
}
