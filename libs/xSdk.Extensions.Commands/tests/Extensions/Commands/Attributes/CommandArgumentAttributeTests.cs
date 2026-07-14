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

namespace xSdk.Extensions.Commands.Attributes;

public class CommandArgumentAttributeTests
{
    [Fact]
    public void Constructor_SetsNameCorrectly()
    {
        var attribute = new CommandArgumentAttribute("myArg");

        Assert.Equal("myArg", attribute.Name);
    }

    [Fact]
    public void Constructor_PreservesCase()
    {
        var attribute = new CommandArgumentAttribute("MyArgument");

        Assert.Equal("MyArgument", attribute.Name);
    }

    [Fact]
    public void AttributeUsage_IsLimitedToProperties()
    {
        var usageAttr = typeof(CommandArgumentAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        Assert.NotNull(usageAttr);
        Assert.Equal(AttributeTargets.Property, usageAttr.ValidOn);
    }

    [Fact]
    public void AttributeUsage_AllowMultiple_IsFalse()
    {
        var usageAttr = typeof(CommandArgumentAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        Assert.NotNull(usageAttr);
        Assert.False(usageAttr.AllowMultiple);
    }

    [Fact]
    public void AttributeUsage_Inherited_IsTrue()
    {
        var usageAttr = typeof(CommandArgumentAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        Assert.NotNull(usageAttr);
        Assert.True(usageAttr.Inherited);
    }

    [Fact]
    public void Attribute_CanBeAppliedToProperty()
    {
        var prop = typeof(SampleHandler).GetProperty(nameof(SampleHandler.MyValue));
        var attribute = prop?.GetCustomAttributes(typeof(CommandArgumentAttribute), false)
            .Cast<CommandArgumentAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("value", attribute.Name);
    }

    private sealed class SampleHandler
    {
        [CommandArgument("value")]
        public string? MyValue { get; set; }
    }
}
