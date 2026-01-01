namespace xSdk.Data
{
    public static class DatalayerBuilderExtensions
    {
        public static IDatalayerBuilder UseVault(this IDatalayerBuilder builder, string name, Action<VaultDatabaseSetup> configure)
            => builder.UseVault(name, false, configure);

        public static IDatalayerBuilder UseVault(this IDatalayerBuilder builder, string name, bool enableWrite, Action<VaultDatabaseSetup> configure)
        {
            builder.UseDatabase<VaultDatabase, VaultDatabaseSetup, VaultConnectionBuilder>(name, configure);
            builder.MapRepository<IReadOnlyVaultRepository, ReadOnlyVaultRepository>(name);

            if (enableWrite)
            {
                builder.MapRepository<IVaultRepository, VaultRepository>(name);
            }

            return builder;
        }
    }
}
