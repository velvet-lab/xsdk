using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class StringHelperTests
{
    [Fact]
    public void RemoveSpecialChars_WithAlphanumericString_ReturnsUnchanged()
    {
        var input = "abc123XYZ";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().Be("abc123XYZ");
    }

    [Fact]
    public void RemoveSpecialChars_WithSpecialCharacters_RemovesThemCorrectly()
    {
        var input = "Hello@World#2024!";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().Be("HelloWorld2024");
    }

    [Fact]
    public void RemoveSpecialChars_WithSpaces_RemovesSpaces()
    {
        var input = "Hello World 123";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().Be("HelloWorld123");
    }

    [Fact]
    public void RemoveSpecialChars_WithUnicodeCharacters_RemovesNonAlphanumeric()
    {
        var input = "Café-123-Ü";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().Be("Caf123");
    }

    [Fact]
    public void RemoveSpecialChars_WithEmptyString_ReturnsEmptyString()
    {
        var input = string.Empty;

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().BeEmpty();
    }

    [Fact]
    public void RemoveSpecialChars_WithOnlySpecialCharacters_ReturnsEmptyString()
    {
        var input = "@#$%^&*()!";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().BeEmpty();
    }

    [Fact]
    public void RemoveSpecialChars_WithMixedCase_PreservesCase()
    {
        var input = "AbC-123-xYz";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().Be("AbC123xYz");
    }

    [Fact]
    public void RemoveSpecialChars_WithUnderscoreAndDash_RemovesBoth()
    {
        var input = "test_value-123";

        var result = StringHelper.RemoveSpecialChars(input);

        result.Should().Be("testvalue123");
    }
}
