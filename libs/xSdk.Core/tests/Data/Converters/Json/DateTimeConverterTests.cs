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

namespace xSdk.Data.Converters.Json;

public class DateTimeConverterTests
{
    private readonly JsonSerializerOptions _options;

    public DateTimeConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new DateTimeConverter());
    }

    [Fact]
    public void Write_SerializesDateTimeInExpectedFormat()
    {
        var dt = new DateTime(2024, 3, 15, 10, 30, 00);
        var json = JsonSerializer.Serialize(dt, _options);

        Assert.Equal("\"2024-03-15 10:30:00\"", json);
    }

    [Fact]
    public void Read_ParsesIsoDateTimeString()
    {
        var json = "\"2024-03-15T10:30:00\"";

        var result = JsonSerializer.Deserialize<DateTime>(json, _options);

        Assert.Equal(new DateTime(2024, 3, 15, 10, 30, 0), result);
    }

    [Fact]
    public void Read_ParsesCustomFormattedString()
    {
        var json = "\"2024-03-15 10:30:00\"";

        var result = JsonSerializer.Deserialize<DateTime>(json, _options);

        Assert.Equal(new DateTime(2024, 3, 15, 10, 30, 0), result);
    }

    [Fact]
    public void Read_WithEmptyString_ReturnsMinValue()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<DateTime>(json, _options);

        Assert.Equal(DateTime.MinValue, result);
    }

    [Fact]
    public void Read_WithNullString_ReturnsMinValue()
    {
        var json = "null";

        var result = JsonSerializer.Deserialize<DateTime?>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void RoundTrip_SerializeDeserialize_RetainsValue()
    {
        var original = new DateTime(2024, 6, 1, 12, 0, 0);

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<DateTime>(json, _options);

        Assert.Equal(original, result);
    }
}
