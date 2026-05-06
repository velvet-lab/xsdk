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

public class ObjectToolsTests
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

        int hash = ObjectTools.CreateAutomaticHashCode(obj);

        Assert.NotEqual(0, hash);
    }

    [Fact]
    public void CreateAutomaticHashCode_WithSameValues_ReturnsSameHash()
    {
        var obj1 = new SimpleObject { Name = "Test", Value = 42 };
        var obj2 = new SimpleObject { Name = "Test", Value = 42 };

        int hash1 = ObjectTools.CreateAutomaticHashCode(obj1);
        int hash2 = ObjectTools.CreateAutomaticHashCode(obj2);

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void CreateHashCode_WithStringValue_ReturnsHash()
    {
        int hash = ObjectTools.CreateHashCode("Hello");

        Assert.NotEqual(0, hash);
    }

    [Fact]
    public void CreateHashCode_WithSameString_ReturnsSameHash()
    {
        int hash1 = ObjectTools.CreateHashCode("test");
        int hash2 = ObjectTools.CreateHashCode("test");

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void CreateHashCode_WithNullValue_ReturnsZero()
    {
        int hash = ObjectTools.CreateHashCode<string>(null);

        Assert.Equal(0, hash);
    }

    [Fact]
    public void CreateHashCode_WithInteger_ReturnsNonZeroHash()
    {
        int hash = ObjectTools.CreateHashCode(42);

        Assert.NotEqual(0, hash);
    }

    private class FieldObject
    {
        public string Name;
        public int Value;
    }

    [Fact]
    public void CreateAutomaticHashCode_WithFields_ReturnsNonZeroHash()
    {
        var obj = new FieldObject { Name = "F", Value = 7 };

        int hash = ObjectTools.CreateAutomaticHashCode(obj);

        Assert.NotEqual(0, hash);
    }

    [Fact]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        bool result = ObjectTools.Equals<int>(42, 42, (a, b) => a == b);

        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        bool result = ObjectTools.Equals<int>(42, 43, (a, b) => a == b);

        Assert.False(result);
    }

    [Fact]
    public void Equals_WithIncompatibleTypes_ReturnsFalse()
    {
        bool result = ObjectTools.Equals<int>("not-an-int", 42, (a, b) => a == b);

        Assert.False(result);
    }

    [Fact]
    public void Equals_WithStringValues_UsesComparer()
    {
        bool result = ObjectTools.Equals<string>("hello", "hello", string.Equals);

        Assert.True(result);
    }
}
