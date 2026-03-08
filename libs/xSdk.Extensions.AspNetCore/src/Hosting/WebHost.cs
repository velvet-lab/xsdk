using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable;

namespace xSdk.Hosting;

public static partial class WebHost
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static IHostBuilder CreateBuilder(string[] args) => CreateBuilder(args, default, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName) => CreateBuilder(args, appName, default, default);

    public static IHostBuilder CreateBuilder(string[] args, string appName, string appPrefix) => CreateBuilder(args, appName, default, appPrefix);

    public static IHostBuilder CreateBuilder(string[] args, string? appName, string? appCompany, string? appPrefix)
    {
        var builder = xSdk
            .Hosting.Host.CreateBuilder(args, appName, appCompany, appPrefix)
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                Logger.Debug("Configuring WebHostBuilder");

                var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
                var stage = envSetup.Stage;

                var contentRoot = GetContentRoot(envSetup);
                webHostBuilder
                    // Set the Content Root
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(contentRoot)

                    // Set the Environment
                    .UseEnvironment(stage.ToString())
                    // Enable detailed Errors if in Development Mode
                    .UseSetting(WebHostDefaults.DetailedErrorsKey, (stage == Stage.Development).ToString())
                    // Configure Services
                    .ConfigureServices(ConfigureWebHostServicesWithContext)
                    // Load Middlewares
                    .Configure(ConfigureApplicationWithContext)
                    // Configure Kestrel
                    .UseKestrel(ConfigureKestrel);

                if (stage == Stage.Development)
                    webHostBuilder.CaptureStartupErrors(true);
            });

        SlimHost.Instance.VariableSystem.RegisterSetup<WebHostSetup>();
        return builder;
    }

    private static string GetContentRoot(EnvironmentSetup envSetup)
    {
        Logger.Debug(envSetup.IsDemo ? "Demo Mode" : "Production Mode");
        Logger.Debug("Try to get Content Root");

        var root = envSetup.ContentRoot;
        if (envSetup.IsDemo)
        {
            return FileSystemHelper.GetExecutingFolder();
        }

        if (!Directory.Exists(root))
        {
            try
            {
                Logger.Trace("Content root does not exist, creating it");
                Directory.CreateDirectory(root);
            }
            catch
            {
                // Only catch, nothing to tell
            }
        }
        return Path.GetFullPath(root);
    }
}
