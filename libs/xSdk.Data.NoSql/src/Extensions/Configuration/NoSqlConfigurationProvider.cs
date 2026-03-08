using LiteDB;

namespace xSdk.Extensions.Configuration;

public sealed class NoSqlConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
{
    private readonly ConnectionString _connectionString;

    internal NoSqlConfigurationProvider(ConnectionString connectionString)
    {
        this._connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public override void Load() { }

    public override void Set(string key, string value) { }

    public override bool TryGet(string key, out string value)
    {
        return base.TryGet(key, out value);
    }

    public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
    {
        return base.GetChildKeys(earlierKeys, parentPath);
    }
}
