using System.Collections.Concurrent;
using NLog;
using xSdk.Shared;

namespace xSdk.Data;

public abstract class Database : IDatabase
{
    private static ConcurrentDictionary<string, object> _connections;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private IDatabaseSetup _setup;
    private string _name;
    private IConnectionBuilder _connectionStringBuilder;

    private static bool _wait4Connection;

    public Database()
    {
        _connections = new ConcurrentDictionary<string, object>();

        AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
        {
            Close();
        };
    }

    #region Dispose Handling

    ~Database() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Close();
    }

    #endregion

    public void Close()
    {
        logger.Trace("Try to close Database");

        if (_connections.Any())
        {
            var keys = _connections.Keys;
            foreach (var key in keys)
            {
                if (_connections.Remove(key, out object connection))
                {
                    Disconnect(connection);
                }
            }
        }
        else
            Disconnect();

    }

    public TConnection Open<TConnection>(bool persistConnection = false)
        where TConnection : class
    {
        object connection = default;

        logger.Trace("Try to open Database for Connection '{0}'", typeof(TConnection));
        ConnectionBuilder builder = _connectionStringBuilder as ConnectionBuilder;

        if (persistConnection)
        {
            // Conccurrent is not really threadsafe (see https://resulhsn.medium.com/understanding-of-concurrentdictionary-in-net-3434105ba371)
            // Rewrite it, to make it thread safe
            var uniqueKey = Base64Helper.ConvertToBase64(_name);

            while (_wait4Connection)
            {
                Thread.Sleep(1);
            }

            _wait4Connection = true;

            TryGetConnection4Key(uniqueKey, out connection);
            connection = Open<TConnection>(connection, () => builder.InitializeConnection(_setup));
            _connections.TryAdd(uniqueKey, connection);

            _wait4Connection = false;
        }
        else
        {
            connection = Open<TConnection>(() => builder.InitializeConnection(_setup));
        }

        return connection as TConnection;
    }

    internal void Configure(IConnectionBuilder connectionStringBuilder, InternalDatabaseSetup setup)
    {
        logger.Trace("Configure new Database");

        _connectionStringBuilder = connectionStringBuilder;
        _setup = setup.Setup;
        _name = setup.Name;
    }

    protected virtual void Disconnect() { }

    protected virtual void Disconnect(object connection) { }

    protected virtual TConnection Open<TConnection>(Func<object> connectionStringBuilder)
        where TConnection : class
    {
        return default;
    }

    protected virtual TConnection Open<TConnection>(object? connection, Func<object> connectionStringBuilder)
        where TConnection : class
    {
        return default;
    }

    private bool ExistsKey(string key)
    {
        var hasItems = false;
        if (_connections.Count() != 0)
            hasItems = true;

        return hasItems && _connections.Keys.Any(k => k == key);
    }

    private bool TryGetConnection4Key(string key, out object connection)
    {
        connection = null;
        if (ExistsKey(key))
        {
            connection = _connections.ToArray().Where(x => x.Key == key).Select(x => x.Value).First();
            if (connection != null)
            {
                return true;
            }
        }

        return false;
    }
}
