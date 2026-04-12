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

namespace xSdk.Tools;

public class StringToolsTests
{
    [Fact]
    public void RemoveSpecialChars_WithAlphanumericString_ReturnsUnchanged()
    {
        var input = "abc123XYZ";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal(input, result);
    }

    [Fact]
    public void RemoveSpecialChars_WithSpecialCharacters_RemovesThemCorrectly()
    {
        var input = "Hello@World#2024!";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal("HelloWorld2024", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithSpaces_RemovesSpaces()
    {
        var input = "Hello World 123";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal("HelloWorld123", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithUnicodeCharacters_RemovesNonAlphanumeric()
    {
        var input = "Café-123-Ü";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal("Caf123", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithEmptyString_ReturnsEmptyString()
    {
        var input = string.Empty;

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveSpecialChars_WithOnlySpecialCharacters_ReturnsEmptyString()
    {
        var input = "@#$%^&*()!";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveSpecialChars_WithMixedCase_PreservesCase()
    {
        var input = "AbC-123-xYz";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal("AbC123xYz", result);
    }

    [Fact]
    public void RemoveSpecialChars_WithUnderscoreAndDash_RemovesBoth()
    {
        var input = "test_value-123";

        var result = StringTools.RemoveSpecialChars(input);

        Assert.Equal("testvalue123", result);
    }
}
