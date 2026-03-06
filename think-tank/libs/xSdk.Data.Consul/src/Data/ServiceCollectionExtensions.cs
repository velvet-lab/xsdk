using System;

namespace xSdk.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IDatalayerBuilder UseConsul(this IDatalayerBuilder builder, string name, Action<ConsulDatabaseSetup> configure) =>
            builder.UseDatabase<ConsulDatabase, ConsulDatabaseSetup, ConsulConnectionBuilder>(name, configure);
    }
}
