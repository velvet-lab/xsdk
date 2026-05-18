/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace xSdk.Data;

[ExcludeFromCodeCoverage(Justification = "MongoDB driver settings builder – requires a running MongoDB instance to exercise.")]
public static class MongoDbOptionsExtensions
{
    public static MongoClientSettings? CreateMongoDbClientSettings(this MongoDbOptions options)
    {
        MongoClientSettings? settings;

        string? connectionString = options.Uri;
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = options.Database;
        }

        settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        settings.Credential = MongoCredential.CreatePlainCredential(options.Database, options.Username, options.Password);

        if (options.Hosts == null || options.Hosts.Length == 0)
        {
            throw new InvalidOperationException("The Hosts property must contain at least one host.");
        }

        string? host = options.Hosts[0];
        if (string.IsNullOrEmpty(host))
        {
            throw new InvalidOperationException("At least one host must be specified in the Hosts property.");
        }

        settings.Server = new MongoServerAddress(host);

        return settings;
    }
}
