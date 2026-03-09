using System.Text;
using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class Base64HelperTests
{
    [Fact]
    public void ConvertToBase64_WithValidString_ReturnsBase64()
    {
        var input = "Hello World";

        var result = Base64Helper.ConvertToBase64(input);

        result.Should().NotBeNullOrEmpty();
        result.Should().Be(Convert.ToBase64String(Encoding.UTF8.GetBytes(input)));
    }

    [Fact]
    public void ConvertToBase64_WithEmptyString_ReturnsBase64OfEmptyString()
    {
        var input = string.Empty;

        var result = Base64Helper.ConvertToBase64(input);

        result.Should().Be(Convert.ToBase64String(Encoding.UTF8.GetBytes("")));
    }

    [Fact]
    public void ConvertToBase64_WithNull_ReturnsBase64OfEmptyString()
    {
        string input = null;

        var result = Base64Helper.ConvertToBase64(input);

        result.Should().Be(Convert.ToBase64String(Encoding.UTF8.GetBytes("")));
    }

    [Fact]
    public void ConvertToBase64_WithUnicodeCharacters_ReturnsValidBase64()
    {
        var input = "Café ☕ 日本語";

        var result = Base64Helper.ConvertToBase64(input);

        result.Should().NotBeNullOrEmpty();
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(result));
        decoded.Should().Be(input);
    }

    [Fact]
    public void ConvertToBase64_WithSpecificEncoding_ReturnsCorrectBase64()
    {
        var input = "Test String";

        var resultUtf8 = Base64Helper.ConvertToBase64(input, Encoding.UTF8);
        var resultUnicode = Base64Helper.ConvertToBase64(input, Encoding.Unicode);

        resultUtf8.Should().NotBe(resultUnicode); // Different encodings produce different base64
    }

    [Fact]
    public void ConvertFromBase64_WithValidBase64_ReturnsOriginalString()
    {
        var original = "Hello World";
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(original));

        var result = Base64Helper.ConvertFromBase64(encoded);

        result.Should().Be(original);
    }

    [Fact]
    public void ConvertFromBase64_WithNonBase64String_ReturnsOriginalString()
    {
        var input = "Not a base64 string!";

        var result = Base64Helper.ConvertFromBase64(input);

        result.Should().Be(input);
    }

    [Fact]
    public void ConvertFromBase64_WithEmptyString_ReturnsEmptyString()
    {
        var input = string.Empty;

        var result = Base64Helper.ConvertFromBase64(input);

        result.Should().BeEmpty();
    }

    [Fact]
    public void ConvertFromBase64_WithNull_ReturnsNull()
    {
        string input = null;

        var result = Base64Helper.ConvertFromBase64(input);

        result.Should().BeNull();
    }

    [Fact]
    public void IsBase64_WithValidBase64_ReturnsTrue()
    {
        var input = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello World"));

        var result = Base64Helper.IsBase64(input);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsBase64_WithInvalidBase64_ReturnsFalse()
    {
        var input = "Not base64!!!";

        var result = Base64Helper.IsBase64(input);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsBase64_WithEmptyString_ReturnsFalse()
    {
        var input = string.Empty;

        var result = Base64Helper.IsBase64(input);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsBase64_WithNull_ReturnsFalse()
    {
        string input = null;

        var result = Base64Helper.IsBase64(input);

        result.Should().BeFalse();
    }

    [Fact]
    public void RoundTrip_EncodeThenDecode_ReturnsOriginalString()
    {
        var original = "Test Message 123!";

        var encoded = Base64Helper.ConvertToBase64(original);
        var decoded = Base64Helper.ConvertFromBase64(encoded);

        decoded.Should().Be(original);
    }

    [Fact]
    public void ConvertToBase64_WithLongString_HandlesCorrectly()
    {
        var input = new string('A', 1000);

        var result = Base64Helper.ConvertToBase64(input);

        result.Should().NotBeNullOrEmpty();
        Base64Helper.IsBase64(result).Should().BeTrue();
    }

    [Fact]
    public void ConvertFromBase64_WithUnicodeEncoding_DecodesCorrectly()
    {
        var original = "Test Unicode";
        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(original));

        var result = Base64Helper.ConvertFromBase64(encoded);

        result.Should().Be(original);
    }
}
