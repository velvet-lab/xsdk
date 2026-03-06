using System;
using Consul;
using Microsoft.Extensions.Logging;

namespace xSdk.Data
{
    public class ConsulDatabase : Database
    {
        private readonly ILogger<ConsulDatabase> _logger;

        public ConsulDatabase(ILogger<ConsulDatabase> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder) => Open<TConnection>(null, connectionStringBuilder);

        protected override TConnection Open<TConnection>(object connection, Func<object> connectionStringBuilder)
        {
            IConsulClient client = connection as IConsulClient;

            try
            {
                if (client == null)
                {
                    client = connectionStringBuilder() as IConsulClient;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "A DBConnection could not created. Is the State Server running?");
                throw;
            }

            return client as TConnection;
        }
    }
}
