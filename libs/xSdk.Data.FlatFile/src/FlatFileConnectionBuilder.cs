using Microsoft.Extensions.Logging;

namespace xSdk.Data
{
    internal sealed class FlatFileConnectionBuilder : ConnectionBuilder
    {
        private readonly ILogger<FlatFileConnectionBuilder> _logger;

        public FlatFileConnectionBuilder(ILogger<FlatFileConnectionBuilder> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override object Create(IDatabaseSetup setup)
        {
            return setup;
        }
    }
}
