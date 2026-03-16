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

using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugin.Tests.Extensions.Variable.Attributes;

public class VariableAttributeTests
{
    [Fact]
    public void VariableAttribute_DefaultConstructor_SetsName()
    {
        var attr = new VariableAttribute(name: "my-variable");

        Assert.Equal("my-variable", attr.Name);
    }

    [Fact]
    public void VariableAttribute_WithDefaultValue_SetsDefaultValue()
    {
        var attr = new VariableAttribute(name: "x", defaultValue: "default-val");

        Assert.Equal("default-val", attr.DefaultValue);
    }

    [Fact]
    public void VariableAttribute_WithTemplate_SetsTemplate()
    {
        var attr = new VariableAttribute(name: "x", template: "--x <value>");

        Assert.Equal("--x <value>", attr.Template);
    }

    [Fact]
    public void VariableAttribute_WithHelpText_SetsHelpText()
    {
        var attr = new VariableAttribute(name: "x", helpText: "Some help");

        Assert.Equal("Some help", attr.HelpText);
    }

    [Fact]
    public void VariableAttribute_WithProtect_SetsProtectTrue()
    {
        var attr = new VariableAttribute(name: "x", protect: true);

        Assert.True(attr.Protect);
    }

    [Fact]
    public void VariableAttribute_WithHidden_SetsHiddenTrue()
    {
        var attr = new VariableAttribute(name: "x", hidden: true);

        Assert.True(attr.Hidden);
    }

    [Fact]
    public void VariableAttribute_WithPrefix_SetsPrefix()
    {
        var attr = new VariableAttribute(name: "x", prefix: "myprefix");

        Assert.Equal("myprefix", attr.Prefix);
    }

    [Fact]
    public void VariableAttribute_WithNoPrefix_SetsNoPrefixTrue()
    {
        var attr = new VariableAttribute(name: "x", noPrefix: true);

        Assert.True(attr.NoPrefix);
    }

    [Fact]
    public void VariableAttribute_DefaultProtect_IsFalse()
    {
        var attr = new VariableAttribute(name: "x");

        Assert.False(attr.Protect);
    }

    [Fact]
    public void VariableAttribute_DefaultHidden_IsFalse()
    {
        var attr = new VariableAttribute(name: "x");

        Assert.False(attr.Hidden);
    }

    [Fact]
    public void VariableAttribute_IsValid_NullValue_ReturnsFalse()
    {
        var attr = new VariableAttribute(name: "x");

        Assert.False(attr.IsValid(null));
    }

    [Fact]
    public void VariableAttribute_IsValid_NonNullValue_ReturnsTrue()
    {
        var attr = new VariableAttribute(name: "x");

        Assert.True(attr.IsValid("some-value"));
    }

    [Fact]
    public void VariablePrefixAttribute_StoresPrefix()
    {
        var attr = new VariablePrefixAttribute("my_prefix");

        Assert.Equal("my_prefix", attr.Prefix);
    }

    [Fact]
    public void VariableNoPrefixAttribute_DefaultConstructor_CreatesInstance()
    {
        var attr = new VariableNoPrefixAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void VariableAttribute_WithResourceNames_SetsResourceNames()
    {
        var names = new[] { "app.name", "service.name" };
        var attr = new VariableAttribute(name: "x", resourceNames: names);

        Assert.Equal(names, attr.ResourceNames);
    }
}
