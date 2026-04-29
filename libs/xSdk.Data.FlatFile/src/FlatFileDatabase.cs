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

using CommunityToolkit.Diagnostics;
using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;

namespace xSdk.Data;

public sealed class FlatFileDatabase(ILogger<FlatFileDatabase> logger) : Database(logger)
{
    private IDataStore? _dataStore = null;

    public override TDatabaseObject? Open<TDatabaseObject>()
        where TDatabaseObject : class
    {
        FlatFileDatabaseOptions? setup = GetOptions<FlatFileDatabaseOptions>(OptionsScope.Datalayer);
        if (setup != null)
        {
            if (_dataStore == null)
            {
                EnsurePathExists(setup.FilePath);

                logger.LogInformation("Initializing flat file database with file path: {FilePath}", setup.FilePath);
                _dataStore = new DataStore(
                    setup.FilePath,
                    useLowerCamelCase: setup.UseLowerCamelCase,
                    keyProperty: setup.KeyProperty,
                    reloadBeforeGetCollection: setup.ReloadBeforeGetCollection,
                    encryptionKey: setup.EncryptionKey,
                    minifyJson: setup.MinifyJson
                );
            }
            else
            {
                logger.LogTrace("Flat file database already initialized, reusing existing instance.");
            }
            return _dataStore as TDatabaseObject;
        }
        else
        {
            logger.LogError("No configuration found for datalayer '{DatalayerName}'. Please ensure that the configuration is properly set up.", this.DatalayerName);
        }

        return default;
    }

    public override bool Close()
    {
        if (_dataStore != null)
        {
            logger.LogInformation("Closing flat file database.");
            _dataStore.Dispose();
            _dataStore = null;
        }
        return true;
    }

    private void EnsurePathExists(string filePath)
    {
        Guard.IsNotNullOrEmpty(filePath);

        string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
        if (!Directory.Exists(directory))
        {
            logger.LogInformation("Creating directory for flat file database at path: {Directory}", directory);
            Directory.CreateDirectory(directory);
        }
    }
}
