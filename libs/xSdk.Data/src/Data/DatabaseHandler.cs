using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;

namespace xSdk.Data;

internal abstract class DatabaseHandler(EnvironmentOptions? environment) : IDatabaseHandler
{
    public string? DatalayerName { get; internal set; }

    internal EnvironmentOptions Environment => environment ?? throw new SdkException("Environment environment are not available.");

    public abstract IDatabase? Retrieve();

    public abstract void Return(IDatabase? database);
}

internal class DatabaseHandler<TDatabase>(ObjectPool<TDatabase> pool, EnvironmentOptions? environment, ILogger<DatabaseHandler<TDatabase>> logger) : DatabaseHandler(environment), IDatabaseHandler<TDatabase>
    where TDatabase : class, IDatabase
{
    public override IDatabase Retrieve()
    {
        logger.LogTrace("Try to open Database");


        var database = pool.Get();        
        if(database is Database concreteDatabase)
        {
            concreteDatabase.DatalayerName = this.DatalayerName;
        }
        return database;
    }

    public override void Return(IDatabase? database)
    {
        if (database != null)
        {
            logger.LogTrace("Try to close Database");
            pool.Return((TDatabase)database);
        }
    }
}
