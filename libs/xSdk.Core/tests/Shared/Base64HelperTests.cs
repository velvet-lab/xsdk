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
using xSdk.Shared;

namespace xSdk.Shared;

public class Base64HelperTests
{
    [Fact]
    public void ConvertToBase64_WithValidString_ReturnsBase64()
    {
        var input = "Hello World";

        var result = Base64Helper.ConvertToBase64(input);

        Assert.NotNull(result);
        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes(input)), result);
    }

    [Fact]
    public void ConvertToBase64_WithEmptyString_ReturnsBase64OfEmptyString()
    {
        var input = string.Empty;

        var result = Base64Helper.ConvertToBase64(input);

        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("")), result);
    }

    [Fact]
    public void ConvertToBase64_WithNull_ReturnsBase64OfEmptyString()
    {
        string input = null;

        var result = Base64Helper.ConvertToBase64(input);

        Assert.Equal(Convert.ToBase64String(Encoding.UTF8.GetBytes("")), result);
    }

    [Fact]
    public void ConvertToBase64_WithUnicodeCharacters_ReturnsValidBase64()
    {
        var input = "Café ☕ 日本語";

        var result = Base64Helper.ConvertToBase64(input);

        Assert.NotNull(result);
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(result));
        Assert.Equal(input, decoded);
    }

    [Fact]
    public void ConvertToBase64_WithSpecificEncoding_ReturnsCorrectBase64()
    {
        var input = "Test String";

        var resultUtf8 = Base64Helper.ConvertToBase64(input, Encoding.UTF8);
        var resultUnicode = Base64Helper.ConvertToBase64(input, Encoding.Unicode);

        Assert.NotEqual(resultUtf8, resultUnicode); // Different encodings produce different base64
    }

    [Fact]
    public void ConvertFromBase64_WithValidBase64_ReturnsOriginalString()
    {
        var original = "Hello World";
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(original));

        var result = Base64Helper.ConvertFromBase64(encoded);

        Assert.Equal(original, result);
    }

    [Fact]
    public void ConvertFromBase64_WithNonBase64String_ReturnsOriginalString()
    {
        var input = "Not a base64 string!";

        var result = Base64Helper.ConvertFromBase64(input);

        Assert.Equal(input, result);
    }

    [Fact]
    public void ConvertFromBase64_WithEmptyString_ReturnsEmptyString()
    {
        var input = string.Empty;

        var result = Base64Helper.ConvertFromBase64(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ConvertFromBase64_WithNull_ReturnsNull()
    {
        string input = null;

        var result = Base64Helper.ConvertFromBase64(input);

        Assert.Null(result);
    }

    [Fact]
    public void IsBase64_WithValidBase64_ReturnsTrue()
    {
        var input = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello World"));

        var result = Base64Helper.IsBase64(input);

        Assert.True(result);
    }

    [Fact]
    public void IsBase64_WithInvalidBase64_ReturnsFalse()
    {
        var input = "Not base64!!!";

        var result = Base64Helper.IsBase64(input);

        Assert.False(result);
    }

    [Fact]
    public void IsBase64_WithEmptyString_ReturnsFalse()
    {
        var input = string.Empty;

        var result = Base64Helper.IsBase64(input);

        Assert.False(result);
    }

    [Fact]
    public void IsBase64_WithNull_ReturnsFalse()
    {
        string input = null;

        var result = Base64Helper.IsBase64(input);

        Assert.False(result);
    }

    [Fact]
    public void RoundTrip_EncodeThenDecode_ReturnsOriginalString()
    {
        var original = "Test Message 123!";

        var encoded = Base64Helper.ConvertToBase64(original);
        var decoded = Base64Helper.ConvertFromBase64(encoded);

        Assert.Equal(original, decoded);
    }

    [Fact]
    public void ConvertToBase64_WithLongString_HandlesCorrectly()
    {
        var input = new string('A', 1000);

        var result = Base64Helper.ConvertToBase64(input);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(Base64Helper.IsBase64(result));
    }

    [Fact]
    public void ConvertFromBase64_WithUnicodeEncoding_DecodesCorrectly()
    {
        var original = "Test Unicode";
        var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(original));

        var result = Base64Helper.ConvertFromBase64(encoded);

        Assert.Equal(original, result);
    }
}
