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

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using xSdk.Tools;

namespace xSdk.Data.Converters.Json;

public sealed class SemVerConverter : JsonConverter<SemVer>
{
    public override SemVer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (Base64Tools.IsBase64(value))
        {
            var converted = Base64Tools.ConvertFromBase64(value);
            var splitted = converted.Split(";", StringSplitOptions.RemoveEmptyEntries);

            return new SemVer(splitted[0], splitted[1]);
        }
        else
        {
            return new SemVer(value);
        }
    }

    public override void Write(Utf8JsonWriter writer, SemVer value, JsonSerializerOptions options)
    {
        var tmp = $"{value.Version};{value.Range}";
        writer.WriteBase64StringValue(Encoding.UTF8.GetBytes(tmp));
    }
}
