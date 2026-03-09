using Microsoft.EntityFrameworkCore;

namespace xSdk.Data;

public sealed class EntityFrameworkDatabase<TDbContext> : Database
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;

    internal EntityFrameworkDatabaseSetup Setup { get; private set; }

    public EntityFrameworkDatabase(IDbContextFactory<TDbContext> factory)
    {
        this._factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder)
    {
        Setup = connectionStringBuilder() as EntityFrameworkDatabaseSetup;
        return _factory.CreateDbContext() as TConnection;
    }

    protected override TConnection Open<TConnection>(object connection, Func<object> connectionStringBuilder)
    {
        Setup = connectionStringBuilder() as EntityFrameworkDatabaseSetup;
        if (connection == null)
        {
            return _factory.CreateDbContext() as TConnection;
        }
        else
        {
            return connection as TConnection;
        }
    }

    protected override void Disconnect(object connection)
    {
        var dbContext = connection as TDbContext;
        dbContext?.Dispose();
    }
}
