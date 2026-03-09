using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class HashToolsTests
{
    [Fact]
    public void GetHash_WithValidString_ReturnsValidByteArray()
    {
        var input = "Hello World";

        var result = HashTools.GetHash(input);

        result.Should().NotBeNull();
        result.Should().HaveCount(32); // SHA256 produces 32 bytes
    }

    [Fact]
    public void GetHash_WithSameInput_ReturnsSameHash()
    {
        var input = "test123";

        var result1 = HashTools.GetHash(input);
        var result2 = HashTools.GetHash(input);

        result1.Should().Equal(result2);
    }

    [Fact]
    public void GetHash_WithDifferentInputs_ReturnsDifferentHashes()
    {
        var input1 = "test123";
        var input2 = "test124";

        var result1 = HashTools.GetHash(input1);
        var result2 = HashTools.GetHash(input2);

        result1.Should().NotEqual(result2);
    }

    [Fact]
    public void GetHash_WithEmptyString_ReturnsValidHash()
    {
        var input = string.Empty;

        var result = HashTools.GetHash(input);

        result.Should().NotBeNull();
        result.Should().HaveCount(32);
    }

    [Fact]
    public void GetHashString_WithValidString_ReturnsHexString()
    {
        var input = "Hello World";

        var result = HashTools.GetHashString(input);

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveLength(64); // 32 bytes * 2 hex chars = 64 chars
        result.Should().MatchRegex("^[0-9A-F]+$"); // Only hex characters (uppercase)
    }

    [Fact]
    public void GetHashString_WithSameInput_ReturnsSameHashString()
    {
        var input = "test123";

        var result1 = HashTools.GetHashString(input);
        var result2 = HashTools.GetHashString(input);

        result1.Should().Be(result2);
    }

    [Fact]
    public void GetHashString_WithDifferentInputs_ReturnsDifferentHashStrings()
    {
        var input1 = "test123";
        var input2 = "test124";

        var result1 = HashTools.GetHashString(input1);
        var result2 = HashTools.GetHashString(input2);

        result1.Should().NotBe(result2);
    }

    [Fact]
    public void GetHashString_WithEmptyString_ReturnsValidHashString()
    {
        var input = string.Empty;

        var result = HashTools.GetHashString(input);

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveLength(64);
    }

    [Fact]
    public void GetHashString_WithKnownInput_ReturnsExpectedHash()
    {
        var input = "test";
        // SHA256 hash of "test" is known
        var expectedHash = "9F86D081884C7D659A2FEAA0C55AD015A3BF4F1B2B0B822CD15D6C15B0F00A08";

        var result = HashTools.GetHashString(input);

        result.Should().Be(expectedHash);
    }

    [Fact]
    public void GetHashString_WithUnicodeCharacters_ReturnsValidHash()
    {
        var input = "Café ☕ 日本語";

        var result = HashTools.GetHashString(input);

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveLength(64);
        result.Should().MatchRegex("^[0-9A-F]+$");
    }
}
