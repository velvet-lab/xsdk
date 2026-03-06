namespace xSdk.Data
{
    public static class IDatalayerBuilderExtensions
    {
        public static IDatalayerBuilder UseNoSql(this IDatalayerBuilder builder, string name, Action<NoSqlDatabaseSetup> configure) =>
            builder.UseDatabase<NoSqlDatabase, NoSqlDatabaseSetup, NoSqlConnectionBuilder>(name, configure);
    }
}
