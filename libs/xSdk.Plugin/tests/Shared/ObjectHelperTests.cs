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

namespace xSdk.Shared;

public class ObjectHelperTests
{
    private class SimpleObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    [Fact]
    public void CreateAutomaticHashCode_WithObjectHavingProperties_ReturnsNonZeroHash()
    {
        var obj = new SimpleObject { Name = "Test", Value = 42 };

        var hash = ObjectHelper.CreateAutomaticHashCode(obj);

        Assert.NotEqual(0, hash);
    }

    [Fact]
    public void CreateAutomaticHashCode_WithSameValues_ReturnsSameHash()
    {
        var obj1 = new SimpleObject { Name = "Test", Value = 42 };
        var obj2 = new SimpleObject { Name = "Test", Value = 42 };

        var hash1 = ObjectHelper.CreateAutomaticHashCode(obj1);
        var hash2 = ObjectHelper.CreateAutomaticHashCode(obj2);

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void CreateHashCode_WithStringValue_ReturnsHash()
    {
        var hash = ObjectHelper.CreateHashCode("Hello");

        Assert.NotEqual(0, hash);
    }

    [Fact]
    public void CreateHashCode_WithSameString_ReturnsSameHash()
    {
        var hash1 = ObjectHelper.CreateHashCode("test");
        var hash2 = ObjectHelper.CreateHashCode("test");

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void CreateHashCode_WithNullValue_ReturnsZero()
    {
        var hash = ObjectHelper.CreateHashCode<string>(null);

        Assert.Equal(0, hash);
    }

    [Fact]
    public void CreateHashCode_WithInteger_ReturnsNonZeroHash()
    {
        var hash = ObjectHelper.CreateHashCode(42);

        Assert.NotEqual(0, hash);
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        var result = ObjectHelper.Equals<int>(42, 42, (a, b) => a == b);

        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        var result = ObjectHelper.Equals<int>(42, 43, (a, b) => a == b);

        Assert.False(result);
    }

    [Fact]
    public void Equals_WithIncompatibleTypes_ReturnsFalse()
    {
        var result = ObjectHelper.Equals<int>("not-an-int", 42, (a, b) => a == b);

        Assert.False(result);
    }

    [Fact]
    public void Equals_WithStringValues_UsesComparer()
    {
        var result = ObjectHelper.Equals<string>("hello", "hello", string.Equals);

        Assert.True(result);
    }
}
