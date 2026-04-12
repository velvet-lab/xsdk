
namespace xSdk.Data;

public interface IDatabaseHandler {

    string? DatalayerName { get; }

    IDatabase? Retrieve();

    void Return(IDatabase? database);
}

public interface IDatabaseHandler<TDatabase> : IDatabaseHandler
    where TDatabase : class, IDatabase
{   
}
