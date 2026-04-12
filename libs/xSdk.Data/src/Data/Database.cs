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

using Microsoft.Extensions.Logging;

namespace xSdk.Data;

public abstract class Database : IDatabase
{
    private ILogger<Database> _logger;

    public string DatalayerName { get; internal set; }

    public Database(ILogger<Database> logger)
    {
        _logger = logger;

        AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
        {
            Close();
        };
    }

    #region Dispose Handling

    ~Database() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Close();
    }

    #endregion

    /// <summary>
    /// Reset the database to a neutral state, semantically similar to when the object was first constructed.
    /// </summary>
    /// <returns><see langword="true" /> if the database was able to reset itself, otherwise <see langword="false" />.</returns>
    /// <remarks>
    /// In general, this method is not expected to be thread-safe.
    /// </remarks>

    public bool TryReset() => Close();

    public abstract TDatabaseObject? Open<TDatabaseObject>()
        where TDatabaseObject : class;

    public virtual bool Close()
    {
        return false;
    }
}
