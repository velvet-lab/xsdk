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

using System.Text.Json;
using xSdk.Tools;

namespace xSdk.Data.Converters.Mapper;

public static class ExtensionDataConverter
{
    public static IDictionary<string, object> Convert(string sourceMember)
    {
        if (!string.IsNullOrEmpty(sourceMember))
        {
            var options = JsonTools.GetSerializerOptions();
            options.WriteIndented = false;

            var result = JsonSerializer.Deserialize<IDictionary<string, object>>(sourceMember, options);
            return result;
        }
        return null;
    }

    public static string Convert(IDictionary<string, object> sourceMember)
    {
        if (sourceMember != null)
        {
            var options = JsonTools.GetSerializerOptions();
            options.WriteIndented = false;

            var json = JsonSerializer.Serialize(sourceMember, options);
            return json;
        }
        return null;
    }
}
