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

public static class JsonElementConverter
{
    public static JsonElement Convert(string sourceMember)
    {
        if (!string.IsNullOrEmpty(sourceMember))
        {
            string? json = Base64Tools.ConvertFromBase64(sourceMember.Trim());
            if (!string.IsNullOrEmpty(json))
            {
                return JsonDocument.Parse(json).RootElement;
            }
        }

        return JsonDocument.Parse("{}").RootElement;
    }

    public static string? Convert(JsonElement sourceMember)
    {
        string json = sourceMember.GetRawText();
        return Base64Tools.ConvertToBase64(json);
    }
}
