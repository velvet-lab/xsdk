namespace xSdk.Data
{
    public interface IDatalayerBuilder
    {
        IDatalayerBuilder ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(string name, Action<TDatabaseSetup> factory)
            where TDatabase : class, IDatabase
            where TDatabaseSetup : IDatabaseSetup, new()
            where TConnectionStringBuilder : class, IConnectionBuilder;
        IDatalayerBuilder ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(string name)
            where TDatabase : class, IDatabase
            where TDatabaseSetup : IDatabaseSetup, new()
            where TConnectionStringBuilder : class, IConnectionBuilder;
        IDatalayerBuilder ConfigureRepository<TImplementation>(IEnumerable<string> dataProviders)
            where TImplementation : class, IRepository;
        IDatalayerBuilder ConfigureRepository<TInterface, TImplementation>(IEnumerable<string> dataProviders)
            where TInterface : class
            where TImplementation : class, IRepository, TInterface;
    }
}
