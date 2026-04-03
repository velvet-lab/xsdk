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

public class StageConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StageConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new StageConverter());
    }

    [Theory]
    [InlineData(Stage.Development, "\"Development\"")]
    [InlineData(Stage.Integration, "\"Integration\"")]
    [InlineData(Stage.Production, "\"Production\"")]
    [InlineData(Stage.None, "\"None\"")]
    public void Write_SerializesStageAsString(Stage stage, string expectedJson)
    {
        var json = JsonSerializer.Serialize(stage, _options);

        Assert.Equal(expectedJson, json);
    }

    [Theory]
    [InlineData("\"Development\"", Stage.Development)]
    [InlineData("\"Integration\"", Stage.Integration)]
    [InlineData("\"Production\"", Stage.Production)]
    [InlineData("\"None\"", Stage.None)]
    public void Read_ParsesStageFromString(string json, Stage expected)
    {
        var result = JsonSerializer.Deserialize<Stage>(json, _options);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Read_EmptyString_ReturnsNone()
    {
        var json = "\"\"";

        var result = JsonSerializer.Deserialize<Stage>(json, _options);

        Assert.Equal(Stage.None, result);
    }

    [Fact]
    public void RoundTrip_SerializeDeserialize_RetainsValue()
    {
        var original = Stage.Production;

        var json = JsonSerializer.Serialize(original, _options);
        var result = JsonSerializer.Deserialize<Stage>(json, _options);

        Assert.Equal(original, result);
    }
}
