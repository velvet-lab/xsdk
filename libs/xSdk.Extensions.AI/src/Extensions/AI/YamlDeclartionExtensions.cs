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

using Microsoft.Agents.ObjectModel;

namespace xSdk.Extensions.AI;

internal static class YamlDeclartionExtensions
{
    extension(RecordDataValue data)
    {
        internal bool TryReadValue(string path, out string? value)
        {
            var propertyPath = PropertyPath.Create(path);
            StringDataValue? property = data.GetProperty<StringDataValue>(propertyPath);

            if (property is not null)
            {
                value = property.Value;
                return true;
            }

            value = default;
            return false;
        }

        internal string? ReadValue(string path)
        {
            data.TryReadValue(path, out string? value);
            return value;
        }
    }
}
