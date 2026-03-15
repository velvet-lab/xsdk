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

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using xSdk.Extensions.Variable;

namespace xSdk.Data;

public abstract class DatabaseSetup : IDatabaseSetup
{
    private readonly Dictionary<string, string> _connectionProperties;
    private readonly ICollection<ValidationResult> _validationResults;

    public DatabaseSetup()
    {
        _connectionProperties = new Dictionary<string, string>();
        _validationResults = new Collection<ValidationResult>();
    }

    public IDictionary<string, string> Properties => _connectionProperties;

    public ICollection<ValidationResult> Results => _validationResults;

    public void Initialize() { }

    public void Validate() => Validate(true);

    public void Validate(bool throwIfFails)
    {
        ValidateSetup();

        if (Results != null && Results.Any())
        {
            var validationResult = Results.ValidateResults();
            if (!validationResult && throwIfFails)
            {
                throw new SdkException("Database Setup is not valid");
            }
        }
    }

    protected virtual void ValidateSetup() { }
}
