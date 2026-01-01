using LiteDB;
using Microsoft.Extensions.Configuration;

namespace xSdk.Extensions.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IConfigurationBuilder AddNoSql(this IConfigurationBuilder builder, ConnectionString connectionString) =>
            builder.Add(new NoSqlConfigurationSource(connectionString));
    }
}
