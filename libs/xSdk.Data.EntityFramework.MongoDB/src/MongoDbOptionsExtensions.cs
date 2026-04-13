using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using xSdk.Hosting;
using xSdk.Shared;

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
