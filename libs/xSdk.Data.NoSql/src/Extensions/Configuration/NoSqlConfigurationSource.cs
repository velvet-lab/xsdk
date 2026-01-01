using LiteDB;
using Microsoft.Extensions.Configuration;

namespace xSdk.Extensions.Configuration
{
    public sealed class NoSqlConfigurationSource : Microsoft.Extensions.Configuration.IConfigurationSource
    {
        private readonly ConnectionString _connectionString;

        internal NoSqlConfigurationSource(ConnectionString connectionString)
        {
            this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new NoSqlConfigurationProvider(_connectionString);
        }
    }
}
