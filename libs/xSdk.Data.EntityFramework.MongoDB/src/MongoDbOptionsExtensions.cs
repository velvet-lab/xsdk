using MongoDB.Driver;

namespace xSdk.Data;

public static class MongoDbOptionsExtensions
{
    public static MongoClientSettings? CreateMongoDbClientSettings(this MongoDbOptions options)
    {
        MongoClientSettings? settings = null;

        var connectionString = options.Uri;
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = options.Database;
        }

        settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

        settings.Credential = MongoCredential.CreatePlainCredential(options.Database, options.Username, options.Password);
        settings.Server = new MongoServerAddress(options.Hosts[0]);

        return settings;
    }
}
