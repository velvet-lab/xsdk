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
using System.Text.Json.Serialization;

namespace xSdk.Data.Converters.Json;

public sealed class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            // GetPrimaryKey Date
            var value = reader.GetString();
            if (!string.IsNullOrEmpty(value))
            {
                if (DateTime.TryParse(value, out DateTime result))
                    return result;

                var splittedDateTime = value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (splittedDateTime.Count() > 1)
                {
                    var splittedDate = splittedDateTime[0].Split("-", StringSplitOptions.RemoveEmptyEntries);
                    var splittedTime = splittedDateTime[1].Split(":", StringSplitOptions.RemoveEmptyEntries);

                    return new DateTime(
                        Convert(splittedDate[0]),
                        Convert(splittedDate[1]),
                        Convert(splittedDate[2]),
                        Convert(splittedTime[0]),
                        Convert(splittedTime[1]),
                        Convert(splittedTime[2])
                    );
                }
            }
            return DateTime.MinValue;
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    private static int Convert(string value) => System.Convert.ToInt32(value);
}
