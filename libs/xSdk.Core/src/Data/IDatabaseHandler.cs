
using xSdk.Extensions.Variable;

namespace xSdk.Data;

public interface IDatabaseHandler : IDatalayerMetadata
{    

    IDatabase? Retrieve();

    void Return(IDatabase? database);
}

public interface IDatabaseHandler<TDatabase> : IDatabaseHandler
    where TDatabase : class, IDatabase
{   
}
