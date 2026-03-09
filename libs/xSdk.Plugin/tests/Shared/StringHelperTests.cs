using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class StringHelperTests
{
    [Fact]
    public void RemoveSpecialChars_WithAlphanumericString_ReturnsUnchanged()
    {
        var input = "abc123XYZ";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal(input, result);
    }

    [Fact]
    public void RemoveSpecialChars_WithSpecialCharacters_RemovesThemCorrectly()
    {
        var input = "Hello@World#2024!";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal("HelloWorld2024", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithSpaces_RemovesSpaces()
    {
        var input = "Hello World 123";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal("HelloWorld123", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithUnicodeCharacters_RemovesNonAlphanumeric()
    {
        var input = "Café-123-Ü";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal("Caf123", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithEmptyString_ReturnsEmptyString()
    {
        var input = string.Empty;

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveSpecialChars_WithOnlySpecialCharacters_ReturnsEmptyString()
    {
        var input = "@#$%^&*()!";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveSpecialChars_WithMixedCase_PreservesCase()
    {
        var input = "AbC-123-xYz";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal("AbC123xYz", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithUnderscoreAndDash_RemovesBoth()
    {
        var input = "test_value-123";

        var result = StringHelper.RemoveSpecialChars(input);

        Assert.Equal("testvalue123", result);
    }
}
