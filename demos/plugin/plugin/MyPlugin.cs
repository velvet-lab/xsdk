using Microsoft.Extensions.DependencyInjection;
using xSdk.Hosting;

namespace xSdk.Demos;

public class MyPlugin : PluginBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Hier können weitere Services konfiguriert werden

        // Wir fügen am besten einen Host hinzu, wenn dieser gestartet wird, dann ist das Framework komplett geladen
        Logger.Info("Add hosted Service");
        services.AddHostedService<PluginHost>();
    }
}
