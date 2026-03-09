using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class TimeSpanParserTests
{
    [Theory]
    [InlineData("100ms", 100)]
    [InlineData("50MS", 50)]
    [InlineData("1ms", 1)]
    public void Parse_WithMilliseconds_ReturnsCorrectTimeSpan(string input, double expectedMilliseconds)
    {
        var result = TimeSpanParser.Parse(input);

        Assert.Equal(TimeSpan.FromMilliseconds(expectedMilliseconds), result);
    }

    [Theory]
    [InlineData("10s", 10)]
    [InlineData("1s", 1)]
    [InlineData("30S", 30)]
    public void Parse_WithSeconds_ReturnsCorrectTimeSpan(string input, double expectedSeconds)
    {
        var result = TimeSpanParser.Parse(input);

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), result);
    }

    [Theory]
    [InlineData("5m", 5)]
    [InlineData("15m", 15)]
    [InlineData("1M", 1)]
    public void Parse_WithMinutes_ReturnsCorrectTimeSpan(string input, double expectedMinutes)
    {
        var result = TimeSpanParser.Parse(input);

        Assert.Equal(TimeSpan.FromMinutes(expectedMinutes), result);
    }

    [Theory]
    [InlineData("2h", 2)]
    [InlineData("24h", 24)]
    [InlineData("1H", 1)]
    public void Parse_WithHours_ReturnsCorrectTimeSpan(string input, double expectedHours)
    {
        var result = TimeSpanParser.Parse(input);

        Assert.Equal(TimeSpan.FromHours(expectedHours), result);
    }

    [Theory]
    [InlineData("1d", 1)]
    [InlineData("7d", 7)]
    [InlineData("30D", 30)]
    public void Parse_WithDays_ReturnsCorrectTimeSpan(string input, double expectedDays)
    {
        var result = TimeSpanParser.Parse(input);

        Assert.Equal(TimeSpan.FromDays(expectedDays), result);
    }

    [Fact]
    public void Parse_WithoutUnit_DefaultsToSeconds()
    {
        var result = TimeSpanParser.Parse("42");

        Assert.Equal(TimeSpan.FromSeconds(42), result);
    }

    [Theory]
    [InlineData("1.5s", 1.5)]
    [InlineData("2.5m", 2.5)]
    [InlineData("0.5h", 0.5)]
    public void Parse_WithDecimalValues_ReturnsCorrectTimeSpan(string input, double expectedValue)
    {
        var result = TimeSpanParser.Parse(input);

        if (input.EndsWith("s"))
            Assert.Equal(TimeSpan.FromSeconds(expectedValue), result);
        else if (input.EndsWith("m"))
            Assert.Equal(TimeSpan.FromMinutes(expectedValue), result);
        else if (input.EndsWith("h"))
            Assert.Equal(TimeSpan.FromHours(expectedValue), result);
    }

    [Fact]
    public void TryParse_WithValidValue_ReturnsTrue()
    {
        var input = "10s";

        var success = TimeSpanParser.TryParse(input, out var result);

        Assert.True(success);
        Assert.Equal(TimeSpan.FromSeconds(10), result);
    }

    [Fact]
    public void TryParse_WithNull_ReturnsFalse()
    {
        var success = TimeSpanParser.TryParse(null, out var result);

        Assert.False(success);
        Assert.Equal(TimeSpan.Zero, result);
    }

    [Fact]
    public void TryParse_WithValidMilliseconds_ReturnsTrueAndCorrectValue()
    {
        var input = "500ms";

        var success = TimeSpanParser.TryParse(input, out var result);

        Assert.True(success);
        Assert.Equal(TimeSpan.FromMilliseconds(500), result);
    }

    [Fact]
    public void TryParse_WithValidMinutes_ReturnsTrueAndCorrectValue()
    {
        var input = "5m";

        var success = TimeSpanParser.TryParse(input, out var result);

        Assert.True(success);
        Assert.Equal(TimeSpan.FromMinutes(5), result);
    }

    [Fact]
    public void TryParse_WithValidHours_ReturnsTrueAndCorrectValue()
    {
        var input = "2h";

        var success = TimeSpanParser.TryParse(input, out var result);

        Assert.True(success);
        Assert.Equal(TimeSpan.FromHours(2), result);
    }

    [Fact]
    public void TryParse_WithValidDays_ReturnsTrueAndCorrectValue()
    {
        var input = "7d";

        var success = TimeSpanParser.TryParse(input, out var result);

        Assert.True(success);
        Assert.Equal(TimeSpan.FromDays(7), result);
    }

    [Fact]
    public void Parse_WithZeroValue_ReturnsZeroTimeSpan()
    {
        var result = TimeSpanParser.Parse("0s");

        Assert.Equal(TimeSpan.Zero, result);
    }

    [Theory]
    [InlineData("100")]
    [InlineData("50s")]
    [InlineData("10m")]
    public void Parse_CaseInsensitive_ParsesCorrectly(string input)
    {
        var resultLower = TimeSpanParser.Parse(input.ToLower());
        var resultUpper = TimeSpanParser.Parse(input.ToUpper());

        Assert.Equal(resultLower, resultUpper);
    }
}
