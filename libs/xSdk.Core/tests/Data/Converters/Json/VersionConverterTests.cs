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
using xSdk.Data.Converters.Json;

namespace xSdk.Data.Converters.Json;

public class VersionConverterTests
{
    private readonly JsonSerializerOptions _options;

    public VersionConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new VersionConverter());
    }

    [Fact]
    public void Write_SerializesVersionAsString()
    {
        var version = new Version(1, 2, 3, 4);

        var json = JsonSerializer.Serialize(version, _options);

        Assert.Equal("\"1.2.3.4\"", json);
    }

    [Fact]
    public void Read_ParsesVersionFromString()
    {
        var json = "\"1.2.3\"";

        var result = JsonSerializer.Deserialize<Version>(json, _options);

        Assert.NotNull(result);
        Assert.Equal(1, result.Major);
        Assert.Equal(2, result.Minor);
        Assert.Equal(3, result.Build);
    }

    [Fact]
    public void Read_WithEmptyString_ReturnsNull()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<Version>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void RoundTrip_SerializeDeserialize_RetainsValue()
    {
        var original = new Version(2, 5, 1);

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<Version>(json, _options);

        Assert.Equal(original, result);
    }
}
