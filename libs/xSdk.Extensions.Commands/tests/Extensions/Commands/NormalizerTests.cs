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

namespace xSdk.Extensions.Commands;

public class NormalizerTests
{
    // --- NormalizeOptionName ---

    [Fact]
    public void NormalizeOptionName_WithoutPrefix_AddsDashDash()
    {
        var result = Normalizer.NormalizeOptionName("verbose");

        Assert.Equal("--verbose", result);
    }

    [Fact]
    public void NormalizeOptionName_AlreadyHasPrefix_ReturnsUnchanged()
    {
        var result = Normalizer.NormalizeOptionName("--verbose");

        Assert.Equal("--verbose", result);
    }

    [Fact]
    public void NormalizeOptionName_EmptyString_ReturnsEmptyString()
    {
        var result = Normalizer.NormalizeOptionName(string.Empty);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void NormalizeOptionName_NullString_ReturnsNull()
    {
        var result = Normalizer.NormalizeOptionName(null!);

        Assert.Null(result);
    }

    // --- NormalizeOptionAliases ---

    [Fact]
    public void NormalizeOptionAliases_WithoutPrefix_AddsSingleDash()
    {
        var result = Normalizer.NormalizeOptionAliases(["v"]);

        Assert.Single(result);
        Assert.Equal("-v", result[0]);
    }

    [Fact]
    public void NormalizeOptionAliases_AlreadyHasPrefix_ReturnsUnchanged()
    {
        var result = Normalizer.NormalizeOptionAliases(["-v"]);

        Assert.Single(result);
        Assert.Equal("-v", result[0]);
    }

    [Fact]
    public void NormalizeOptionAliases_EmptyArray_ReturnsEmptyArray()
    {
        var result = Normalizer.NormalizeOptionAliases([]);

        Assert.Empty(result);
    }

    [Fact]
    public void NormalizeOptionAliases_EmptyStringEntry_IsSkipped()
    {
        var result = Normalizer.NormalizeOptionAliases([string.Empty]);

        Assert.Empty(result);
    }

    [Fact]
    public void NormalizeOptionAliases_MixedPrefixes_NormalizesCorrectly()
    {
        var result = Normalizer.NormalizeOptionAliases(["v", "-x", "verbose"]);

        Assert.Equal(3, result.Length);
        Assert.Equal("-v", result[0]);
        Assert.Equal("-x", result[1]);
        Assert.Equal("-verbose", result[2]);
    }
}
