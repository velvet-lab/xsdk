using System.Text.Json;
using xSdk.Data;

namespace xSdk.Plugin.Tests.Data;

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
