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

using xSdk.Shared;

namespace xSdk.Extensions.Variable.Providers;

internal sealed class FileProvider : VariableProviderBase
{
    protected override bool ExistsVariable(IVariable variable)
    {
        if (variable != null)
        {
            var fileName = EnvironmentTools.ReadEnvironmentVariable(Cast(variable).KeyForFile);
            if (!string.IsNullOrEmpty(fileName))
                return File.Exists(fileName);
        }

        return false;
    }

    protected override object? ReadVariable(IVariable variable)
    {
        string? result = default;

        var fileName = EnvironmentTools.ReadEnvironmentVariable(Cast(variable).KeyForFile);
        if (!string.IsNullOrEmpty(fileName))
        {
            if (File.Exists(fileName))
            {
                result = File.ReadAllText(fileName);
            }
        }

        return result;
    }
}
