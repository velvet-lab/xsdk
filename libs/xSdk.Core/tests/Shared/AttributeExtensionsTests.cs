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

using System.Reflection;
using xSdk.Shared;

namespace xSdk.Shared;

public class AttributeExtensionsTests
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    private class TestAttribute(string value) : Attribute
    {
        public string Value { get; } = value;
    }

    [AttributeUsage(AttributeTargets.Class)]
    private class AnotherAttribute : Attribute { }

    [TestAttribute("class-value")]
    private class AnnotatedClass
    {
        [TestAttribute("prop-value")]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }
    }

    private class UnannotatedClass
    {
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void GetAttribute_OnType_WithMatchingAttribute_ReturnsAttribute()
    {
        var attr = typeof(AnnotatedClass).GetAttribute<TestAttribute>();

        Assert.NotNull(attr);
        Assert.Equal("class-value", attr.Value);
    }

    [Fact]
    public void GetAttribute_OnType_WithoutMatchingAttribute_ReturnsNull()
    {
        var attr = typeof(UnannotatedClass).GetAttribute<TestAttribute>();

        Assert.Null(attr);
    }

    [Fact]
    public void GetAttribute_OnProperty_WithMatchingAttribute_ReturnsAttribute()
    {
        var propertyInfo = typeof(AnnotatedClass).GetProperty(nameof(AnnotatedClass.Name))!;

        var attr = propertyInfo.GetAttribute<TestAttribute>();

        Assert.NotNull(attr);
        Assert.Equal("prop-value", attr.Value);
    }

    [Fact]
    public void GetAttribute_OnProperty_WithoutMatchingAttribute_ReturnsNull()
    {
        var propertyInfo = typeof(AnnotatedClass).GetProperty(nameof(AnnotatedClass.Age))!;

        var attr = propertyInfo.GetAttribute<TestAttribute>();

        Assert.Null(attr);
    }

    [Fact]
    public void GetAttribute_OnType_WithDifferentAttributeType_ReturnsNull()
    {
        var attr = typeof(AnnotatedClass).GetAttribute<AnotherAttribute>();

        Assert.Null(attr);
    }
}
