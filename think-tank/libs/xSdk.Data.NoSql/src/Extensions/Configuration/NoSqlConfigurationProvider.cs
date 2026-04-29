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
