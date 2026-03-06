using xSdk.Extensions.Variable;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace xSdk.Demos.Hosting
{
    public class MyCustomHost : IHostedService
    {
        private readonly IVariableService _variableSvc;
        private readonly ILogger<MyCustomHost> _logger;
        private readonly SetupWithoutPrefix _setupWithoutPrefix;
        private readonly SetupWithPrefix _setupWithPrefix;

        public MyCustomHost(IVariableService variableSvc, ILogger<MyCustomHost> logger)
        {
            this._variableSvc = variableSvc ?? throw new ArgumentNullException(nameof(variableSvc));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this._setupWithoutPrefix = variableSvc.GetSetup<SetupWithoutPrefix>();
            this._setupWithPrefix = variableSvc.GetSetup<SetupWithPrefix>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            _logger.LogInformation("Host was started");

            _logger.LogInformation("{0} = {1}", nameof(this._setupWithPrefix.WithAppPrefix_WithSetupPrefix), this._setupWithPrefix.WithAppPrefix_WithSetupPrefix);
            _logger.LogInformation("{0} = {1}", nameof(this._setupWithPrefix.NoAppPrefix_NoSetupPrefix), this._setupWithPrefix.NoAppPrefix_NoSetupPrefix);

            _logger.LogInformation("{0} = {1}", nameof(this._setupWithoutPrefix.NoAppPrefix_NoSetupPrefix), this._setupWithoutPrefix.NoAppPrefix_NoSetupPrefix);
            _logger.LogInformation("{0} = {1}", nameof(this._setupWithoutPrefix.WithAppPrefix_NoSetupPrefix), this._setupWithoutPrefix.WithAppPrefix_NoSetupPrefix);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
