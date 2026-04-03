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
using xSdk.Data;

namespace xSdk.Data;

public class JsonHelperTests
{
    [Theory]
    [InlineData("{\"key\":\"value\"}", true)]
    [InlineData("{}", true)]
    [InlineData("[1,2,3]", true)]
    [InlineData("[]", true)]
    [InlineData("not json", false)]
    [InlineData("plain text", false)]
    [InlineData("", false)]
    public void IsJson_ReturnsExpectedResult(string input, bool expected)
    {
        var result = JsonHelper.IsJson(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsJson_WithInvalidObject_ReturnsFalse()
    {
        var result = JsonHelper.IsJson("{invalid}");

        Assert.False(result);
    }

    [Fact]
    public void Merge_TwoObjects_CombinesProperties()
    {
        var income = "{\"a\":1}";
        var outcome = "{\"b\":2}";

        var result = JsonHelper.Merge(income, outcome);

        Assert.Contains("\"a\"", result);
        Assert.Contains("\"b\"", result);
    }

    [Fact]
    public void Merge_WithEmptyIncome_ReturnsOutcome()
    {
        var income = "";
        var outcome = "{\"b\":2}";

        var result = JsonHelper.Merge(income, outcome);

        Assert.Contains("\"b\"", result);
    }

    [Fact]
    public void Merge_WithEmptyOutcome_ReturnsIncome()
    {
        var income = "{\"a\":1}";
        var outcome = "";

        var result = JsonHelper.Merge(income, outcome);

        Assert.Contains("\"a\"", result);
    }

    [Fact]
    public void GetSerializerOptions_ReturnsNonNull()
    {
        var options = JsonHelper.GetSerializerOptions();

        Assert.NotNull(options);
    }

    [Fact]
    public void GetSerializerOptions_HasExpectedSettings()
    {
        var options = JsonHelper.GetSerializerOptions();

        Assert.True(options.PropertyNameCaseInsensitive);
        Assert.True(options.AllowTrailingCommas);
    }

    [Fact]
    public void GetSerializerOptions_Compact_DoesNotWriteIndented()
    {
        var options = JsonHelper.GetSerializerOptions(compact: true);

        Assert.False(options.WriteIndented);
    }

    [Fact]
    public void GetSerializerOptions_NonCompact_WritesIndented()
    {
        var options = JsonHelper.GetSerializerOptions(compact: false);

        Assert.True(options.WriteIndented);
    }

    [Fact]
    public void ConfigureSerializerOptions_AddsConverters()
    {
        var options = new JsonSerializerOptions();

        options.ConfigureSerializerOptions();

        Assert.NotEmpty(options.Converters);
    }

    [Fact]
    public void GetDocumentOptions_ReturnsNonNull()
    {
        var options = JsonHelper.GetDocumentOptions();

        Assert.Equal(JsonCommentHandling.Skip, options.CommentHandling);
        Assert.True(options.AllowTrailingCommas);
    }
}
