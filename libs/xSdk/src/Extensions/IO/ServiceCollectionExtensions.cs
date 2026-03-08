using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xSdk.Hosting;

namespace xSdk.Extensions.IO;

public static class ServiceCollectionExtensions
{
    private static bool IsLocked;

    public static IServiceCollection AddFileServices(this IServiceCollection services)
    {
        services.TryAddSingleton(provider =>
        {
            IsLocked = true;
            return SlimHost.Instance.FileSystem;
        });

        return services;
    }

    internal static IServiceCollection AddSlimFileServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IFileSystemService>(provider =>
        {
            if (!IsLocked)
            {
                return ActivatorUtilities.CreateInstance<FileSystemService>(provider);
            }
            else
            {
                throw new SdkException("FileServices are locked and cannot be used anymore over SlimHost");
            }
        });

        return services;
    }
}
