using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Data;

public static class MongoDbSetupExtensions
{
    public static MongoClientSettings? CreateMongoDbClientSettings(this MongoDbSetup mongoSetup, out string databaseName)
    {
        MongoClientSettings? settings = null;
        databaseName = SlimHost.Instance.AppName;
            
        var connectionString = mongoSetup.Uri;
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = mongoSetup.Database;
        }
        settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));        

        return settings;
    }
}
