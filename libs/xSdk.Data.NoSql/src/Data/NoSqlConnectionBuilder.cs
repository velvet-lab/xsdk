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

namespace xSdk.Data;

internal sealed class NoSqlConnectionBuilder : ConnectionBuilder
{
    public override object Create(IDatabaseSetup setup)
    {
        ConnectionString connectionString = default;
        var concreteSetup = (NoSqlDatabaseSetup)setup;

        // Only Direct Mode will work correct
        var mode = ConnectionType.Direct;

        connectionString = new ConnectionString()
        {
            Upgrade = concreteSetup.Upgrade,
            Connection = mode,
            ReadOnly = concreteSetup.ReadOnly,
            Collation = concreteSetup.Collation,
        };

        if (!string.IsNullOrEmpty(concreteSetup.FileName))
        {
            var fileName = concreteSetup.FileName;
            fileName = ResolvePlaceholders(fileName);

            if (!fileName.EndsWith(".db"))
                fileName += ".db";

            if (!Directory.Exists(concreteSetup.Path))
                Directory.CreateDirectory(concreteSetup.Path);

            connectionString.Filename = Path.Combine(concreteSetup.Path, fileName);
        }

        if (!string.IsNullOrEmpty(concreteSetup.Password))
            connectionString.Password = concreteSetup.Password;

        if (concreteSetup.InitialSize > 0)
            connectionString.InitialSize = concreteSetup.InitialSize;

        var fileInfo = new FileInfo(connectionString.Filename);
        if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            fileInfo.Directory.Create();

        if (connectionString == null)
            throw new SdkException("No Connection String given to open LiteDb");

        return connectionString;
    }
}
