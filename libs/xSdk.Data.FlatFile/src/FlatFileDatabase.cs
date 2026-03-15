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

using JsonFlatFileDataStore;

namespace xSdk.Data;

public sealed class FlatFileDatabase : Database
{
    protected override TConnection Open<TConnection>(Func<object> connectionStringBuilder) => Open<TConnection>(null, connectionStringBuilder);

    protected override TConnection Open<TConnection>(object connection, Func<object> connectionStringBuilder)
    {
        var setup = connectionStringBuilder() as FlatFileDatabaseSetup;

        var datastore = new DataStore(
            setup.FilePath,
            useLowerCamelCase: setup.UseLowerCamelCase,
            keyProperty: setup.KeyProperty,
            reloadBeforeGetCollection: setup.ReloadBeforeGetCollection,
            encryptionKey: setup.EncryptionKey
        );

        return datastore as TConnection;
    }
}
