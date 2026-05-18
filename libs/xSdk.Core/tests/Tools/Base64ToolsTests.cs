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

using System.Text;

namespace xSdk.Tools;

public class Base64ToolsTests
{
    [Fact]
    public void ConvertToBase64_WithValidString_ReturnsBase64()
    {
        string input = "Hello World";

        string? result = Base64Tools.ConvertToBase64(input);

        Assert.NotNull(result);
        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes(input)), result);
    }

    [Fact]
    public void ConvertToBase64_WithEmptyString_ReturnsBase64OfEmptyString()
    {
        string input = string.Empty;

        string? result = Base64Tools.ConvertToBase64(input);

        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("")), result);
    }

    [Fact]
    public void ConvertToBase64_WithNull_ReturnsBase64OfEmptyString()
    {
        string? input = null;

        string? result = Base64Tools.ConvertToBase64(input);

        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("")), result);
    }

    [Fact]
    public void ConvertToBase64_WithUnicodeCharacters_ReturnsValidBase64()
    {
        string input = "Café ☕ 日本語";

        string? result = Base64Tools.ConvertToBase64(input);

        Assert.NotNull(result);
        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(result));
        Assert.Equal(input, decoded);
    }

    [Fact]
    public void ConvertToBase64_WithSpecificEncoding_ReturnsCorrectBase64()
    {
        string input = "Test String";

        string? resultUtf8 = Base64Tools.ConvertToBase64(input, Encoding.UTF8);
        string? resultUnicode = Base64Tools.ConvertToBase64(input, Encoding.Unicode);

        Assert.NotEqual(resultUtf8, resultUnicode); // Different encodings produce different base64
    }

    [Fact]
    public void ConvertFromBase64_WithValidBase64_ReturnsOriginalString()
    {
        string original = "Hello World";
        string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(original));

        string? result = Base64Tools.ConvertFromBase64(encoded);

        Assert.Equal(original, result);
    }

    [Fact]
    public void ConvertFromBase64_WithNonBase64String_ReturnsOriginalString()
    {
        string input = "Not a base64 string!";

        string? result = Base64Tools.ConvertFromBase64(input);

        Assert.Equal(input, result);
    }

    [Fact]
    public void ConvertFromBase64_WithEmptyString_ReturnsEmptyString()
    {
        string input = string.Empty;

        string? result = Base64Tools.ConvertFromBase64(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ConvertFromBase64_WithNull_ReturnsNull()
    {
        string? input = null;

        string? result = Base64Tools.ConvertFromBase64(input);

        Assert.Null(result);
    }

    [Fact]
    public void IsBase64_WithValidBase64_ReturnsTrue()
    {
        string input = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello World"));

        bool result = Base64Tools.IsBase64(input);

        Assert.True(result);
    }

    [Fact]
    public void IsBase64_WithInvalidBase64_ReturnsFalse()
    {
        string input = "Not base64!!!";

        bool result = Base64Tools.IsBase64(input);

        Assert.False(result);
    }

    [Fact]
    public void IsBase64_WithEmptyString_ReturnsFalse()
    {
        string input = string.Empty;

        bool result = Base64Tools.IsBase64(input);

        Assert.False(result);
    }

    [Fact]
    public void IsBase64_WithNull_ReturnsFalse()
    {
        string? input = null;

        bool result = Base64Tools.IsBase64(input);

        Assert.False(result);
    }

    [Fact]
    public void RoundTrip_EncodeThenDecode_ReturnsOriginalString()
    {
        string original = "Test Message 123!";

        string? encoded = Base64Tools.ConvertToBase64(original);
        string? decoded = Base64Tools.ConvertFromBase64(encoded);

        Assert.Equal(original, decoded);
    }

    [Fact]
    public void ConvertToBase64_WithLongString_HandlesCorrectly()
    {
        string input = new string('A', 1000);

        string? result = Base64Tools.ConvertToBase64(input);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(Base64Tools.IsBase64(result));
    }

    [Fact]
    public void ConvertFromBase64_WithUnicodeEncoding_DecodesCorrectly()
    {
        string original = "Test Unicode";
        string encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(original));

        string? result = Base64Tools.ConvertFromBase64(encoded);

        Assert.Equal(original, result);
    }

    [Fact]
    public void ConvertFromBase64_WithNullByteEncoded_ReturnsOriginalEncoded()
    {
        string encoded = Convert.ToBase64String(new byte[] { 0 });

        string? result = Base64Tools.ConvertFromBase64(encoded);

        Assert.Equal(encoded, result);
    }

    [Fact]
    public void IsBase64_WithNullByteDecoded_ReturnsFalse()
    {
        string encoded = Convert.ToBase64String(new byte[] { 0 });

        bool result = Base64Tools.IsBase64(encoded);

        Assert.False(result);
    }
}
