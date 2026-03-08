namespace xSdk.Data;

public static class ServiceCollectionExtensions
{
    public static IDatalayerBuilder UseFlatFile(this IDatalayerBuilder builder, string name, Action<FlatFileDatabaseSetup> configure) =>
        builder.UseDatabase<FlatFileDatabase, FlatFileDatabaseSetup, FlatFileConnectionBuilder>(name, configure);
}
