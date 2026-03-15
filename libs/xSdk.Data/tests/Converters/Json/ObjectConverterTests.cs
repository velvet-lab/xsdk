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

namespace xSdk.Data.Tests.Converters.Json;

public class ObjectConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ObjectConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new ObjectConverter());
    }

    [Fact]
    public void Read_StringToken_ReturnsStringValue()
    {
        var json = "\"hello world\"";

        var result = JsonSerializer.Deserialize<object>(json, _options);

        Assert.Equal("hello world", result);
    }

    [Fact]
    public void Read_EmptyString_ReturnsNull()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<object>(json, _options);

        Assert.Null(result);
    }

    [Fact]
    public void Write_StringValue_SerializesAsString()
    {
        object value = "test-value";

        var json = JsonSerializer.Serialize(value, _options);

        Assert.Equal("\"test-value\"", json);
    }
}
