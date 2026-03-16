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

using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class HashToolsTests
{
    [Fact]
    public void GetHash_WithValidString_ReturnsValidByteArray()
    {
        var input = "Hello World";

        var result = HashTools.GetHash(input);

        Assert.NotNull(result);
        Assert.Equal(32, result.Length); // SHA256 produces 32 bytes
    }

    [Fact]
    public void GetHash_WithSameInput_ReturnsSameHash()
    {
        var input = "test123";

        var result1 = HashTools.GetHash(input);
        var result2 = HashTools.GetHash(input);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void GetHash_WithDifferentInputs_ReturnsDifferentHashes()
    {
        var input1 = "test123";
        var input2 = "test124";

        var result1 = HashTools.GetHash(input1);
        var result2 = HashTools.GetHash(input2);

        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void GetHash_WithEmptyString_ReturnsValidHash()
    {
        var input = string.Empty;

        var result = HashTools.GetHash(input);

        Assert.NotNull(result);
        Assert.Equal(32, result.Length);
    }

    [Fact]
    public void GetHashString_WithValidString_ReturnsHexString()
    {
        var input = "Hello World";

        var result = HashTools.GetHashString(input);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(64, result.Length); // 32 bytes * 2 hex chars = 64 chars
        Assert.Matches("^[0-9A-F]+$", result); // Only hex characters (uppercase)
    }

    [Fact]
    public void GetHashString_WithSameInput_ReturnsSameHashString()
    {
        var input = "test123";

        var result1 = HashTools.GetHashString(input);
        var result2 = HashTools.GetHashString(input);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void GetHashString_WithDifferentInputs_ReturnsDifferentHashStrings()
    {
        var input1 = "test123";
        var input2 = "test124";

        var result1 = HashTools.GetHashString(input1);
        var result2 = HashTools.GetHashString(input2);

        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void GetHashString_WithEmptyString_ReturnsValidHashString()
    {
        var input = string.Empty;

        var result = HashTools.GetHashString(input);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(64, result.Length);
    }

    [Fact]
    public void GetHashString_WithKnownInput_ReturnsExpectedHash()
    {
        var input = "test";
        // SHA256 hash of "test" is known
        var expectedHash = "9F86D081884C7D659A2FEAA0C55AD015A3BF4F1B2B0B822CD15D6C15B0F00A08";

        var result = HashTools.GetHashString(input);

        Assert.Equal(expectedHash, result);
    }

    [Fact]
    public void GetHashString_WithUnicodeCharacters_ReturnsValidHash()
    {
        var input = "Café ☕ 日本語";

        var result = HashTools.GetHashString(input);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(64, result.Length);
        Assert.Matches("^[0-9A-F]+$", result);
    }
}
