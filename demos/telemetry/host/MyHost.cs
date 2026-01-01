using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace xSdk.Demos
{
    internal class MyHost : IHostedService
    {
        private readonly LocalService _localSvc;
        private readonly ILogger<MyHost> _logger;

        public MyHost(LocalService localSvc, ILogger<MyHost> logger)
        {
            this._localSvc = localSvc ?? throw new ArgumentNullException(nameof(localSvc));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Now lets start a local service");
            _localSvc.DoWorkA();
            _localSvc.DoWorkB();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
