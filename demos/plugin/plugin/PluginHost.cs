using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace xSdk.Demos
{
    internal class PluginHost(ILogger<PluginHost> _logger) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Demostriere das Logging
            _logger.LogInformation("Demostrate logging");
            _logger.LogCritical("This is a critical Message");
            _logger.LogError("This is a error Message");
            _logger.LogWarning("This is a warning Message");
            _logger.LogInformation("This is a info Message");
            _logger.LogDebug("This is a debug Message");
            _logger.LogTrace("This is a trace Message");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
