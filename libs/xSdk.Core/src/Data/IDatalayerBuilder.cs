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

using xSdk.Extensions.Variable;

namespace xSdk.Data;

public interface IDatalayerBuilder
{
    IDatabaseBuilder ConfigureDatabase<TDatabase, TDatabaseOptions>()
        where TDatabase : class, IDatabase
        where TDatabaseOptions : class, IVariableSetup
        => ConfigureDatabase<TDatabase, TDatabaseOptions>(null, null);

    IDatabaseBuilder ConfigureDatabase<TDatabase, TDatabaseOptions>(Action<TDatabaseOptions>? factory)
        where TDatabase : class, IDatabase
        where TDatabaseOptions : class, IVariableSetup
        => ConfigureDatabase<TDatabase, TDatabaseOptions>(null, factory);

    IDatabaseBuilder ConfigureDatabase<TDatabase, TDatabaseOptions>(string? name)
        where TDatabase : class, IDatabase
        where TDatabaseOptions : class, IVariableSetup
        => ConfigureDatabase<TDatabase, TDatabaseOptions>(name, null);

    IDatabaseBuilder ConfigureDatabase<TDatabase, TDatabaseOptions>(string? name, Action<TDatabaseOptions>? factory)
        where TDatabase : class, IDatabase
        where TDatabaseOptions : class, IVariableSetup;
}
