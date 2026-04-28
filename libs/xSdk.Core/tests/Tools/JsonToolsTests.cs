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

namespace xSdk.Tools;

public class JsonToolsTests
{
    [Theory]
    [InlineData("{\"key\":\"value\"}")]
    [InlineData("{\"a\":1,\"b\":true}")]
    [InlineData("{}")]
    public void IsJson_WithValidJsonObject_ReturnsTrue(string json)
    {
        var result = JsonTools.IsJson(json);

        Assert.True(result);
    }

    [Theory]
    [InlineData("[1,2,3]")]
    [InlineData("[{\"a\":1}]")]
    [InlineData("[]")]
    public void IsJson_WithValidJsonArray_ReturnsTrue(string json)
    {
        var result = JsonTools.IsJson(json);

        Assert.True(result);
    }

    [Theory]
    [InlineData("plain string")]
    [InlineData("12345")]
    [InlineData("true")]
    [InlineData("")]
    public void IsJson_WithNonJsonString_ReturnsFalse(string input)
    {
        var result = JsonTools.IsJson(input);

        Assert.False(result);
    }

    [Theory]
    [InlineData("{invalid json}")]
    [InlineData("{\"key\":}")]
    public void IsJson_WithMalformedJson_ReturnsFalse(string input)
    {
        var result = JsonTools.IsJson(input);

        Assert.False(result);
    }

    [Fact]
    public void Merge_WithTwoValidJsonObjects_ReturnsMergedJson()
    {
        var income = "{\"a\":1}";
        var outcome = "{\"b\":2}";

        var result = JsonTools.Merge(income, outcome);

        Assert.Contains("\"a\":1", result);
        Assert.Contains("\"b\":2", result);
    }

    [Fact]
    public void Merge_WithEmptyIncome_ReturnsOutcomeValues()
    {
        var outcome = "{\"key\":\"value\"}";

        var result = JsonTools.Merge(string.Empty, outcome);

        Assert.Contains("\"key\":\"value\"", result);
    }

    [Fact]
    public void Merge_WithEmptyOutcome_ReturnsIncomeValues()
    {
        var income = "{\"key\":\"value\"}";

        var result = JsonTools.Merge(income, string.Empty);

        Assert.Contains("\"key\":\"value\"", result);
    }

    [Fact]
    public void Merge_WithBothEmpty_ReturnsEmptyObject()
    {
        var result = JsonTools.Merge(string.Empty, string.Empty);

        Assert.Equal("{}", result);
    }

    [Fact]
    public void Merge_WithInvalidIncome_ReturnsEmptyObject()
    {
        var result = JsonTools.Merge("not-json", "{\"b\":2}");

        Assert.Equal("{}", result);
    }

    [Fact]
    public void Merge_WithInvalidOutcome_ReturnsEmptyObject()
    {
        var result = JsonTools.Merge("{\"a\":1}", "not-json");

        Assert.Equal("{}", result);
    }

    [Fact]
    public void Merge_WithOverlappingKeys_OutcomeValueWins()
    {
        var income = "{\"key\":\"original\"}";
        var outcome = "{\"key\":\"overridden\"}";

        var result = JsonTools.Merge(income, outcome);

        Assert.Contains("\"overridden\"", result);
    }

    [Fact]
    public void GetSerializerOptions_ReturnsNonNullOptions()
    {
        var options = JsonTools.GetSerializerOptions();

        Assert.NotNull(options);
    }

    [Fact]
    public void GetSerializerOptions_DefaultIsIndented()
    {
        var options = JsonTools.GetSerializerOptions();

        Assert.True(options.WriteIndented);
    }

    [Fact]
    public void GetSerializerOptions_WithCompact_IsNotIndented()
    {
        var options = JsonTools.GetSerializerOptions(compact: true);

        Assert.False(options.WriteIndented);
    }

    [Fact]
    public void GetSerializerOptions_WithoutCompact_IsIndented()
    {
        var options = JsonTools.GetSerializerOptions(compact: false);

        Assert.True(options.WriteIndented);
    }

    [Fact]
    public void GetSerializerOptions_IsCaseInsensitive()
    {
        var options = JsonTools.GetSerializerOptions();

        Assert.True(options.PropertyNameCaseInsensitive);
    }

    [Fact]
    public void GetSerializerOptions_AllowsTrailingCommas()
    {
        var options = JsonTools.GetSerializerOptions();

        Assert.True(options.AllowTrailingCommas);
    }

    [Fact]
    public void ConfigureSerializerOptions_ReturnsConfiguredOptions()
    {
        var options = new System.Text.Json.JsonSerializerOptions();

        var result = JsonTools.ConfigureSerializerOptions(options);

        Assert.NotNull(result);
        Assert.True(result.AllowTrailingCommas);
        Assert.True(result.PropertyNameCaseInsensitive);
        Assert.True(result.WriteIndented);
    }

    [Fact]
    public void ConfigureSerializerOptions_WithCompact_DisablesIndentation()
    {
        var options = new System.Text.Json.JsonSerializerOptions();

        var result = JsonTools.ConfigureSerializerOptions(options, compact: true);

        Assert.False(result.WriteIndented);
    }

    [Fact]
    public void GetDocumentOptions_ReturnsNonNullOptions()
    {
        var options = JsonTools.GetDocumentOptions();

        Assert.Equal(System.Text.Json.JsonCommentHandling.Skip, options.CommentHandling);
        Assert.True(options.AllowTrailingCommas);
    }
}
