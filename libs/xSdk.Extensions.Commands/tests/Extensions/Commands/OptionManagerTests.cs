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

using System.CommandLine;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using xSdk.Extensions.Commands.Attributes;

namespace xSdk.Extensions.Commands;

public class OptionManagerTests
{
    private sealed class HandlerWithOption : CommandHandler
    {
        [CommandOptionAttribute("verbose", "v"), Description("Enable verbose output")]
        public bool Verbose { get; set; }
    }

    private sealed class HandlerWithRequiredOption : CommandHandler
    {
        [CommandOptionAttribute("output"), Required, Description("Output path")]
        public string? Output { get; set; }
    }

    private sealed class HandlerWithNoOptions : CommandHandler
    {
        public string? PlainProperty { get; set; }
    }

    private sealed class HandlerWithMultipleOptions : CommandHandler
    {
        [CommandOptionAttribute("input", "i"), Description("Input file")]
        public string? Input { get; set; }

        [CommandOptionAttribute("output", "o"), Description("Output file")]
        public string? Output { get; set; }
    }

    [Fact]
    public void Build_TypeWithOptionProperty_ReturnsOption()
    {
        Option[] result = OptionManager.Build(typeof(HandlerWithOption));

        Assert.Single(result);
        Assert.Equal("--verbose", result[0].Name);
    }

    [Fact]
    public void Build_TypeWithNoOptionAttributes_ReturnsEmpty()
    {
        Option[] result = OptionManager.Build(typeof(HandlerWithNoOptions));

        Assert.Empty(result);
    }

    [Fact]
    public void Build_TypeWithMultipleOptions_ReturnsAll()
    {
        Option[] result = OptionManager.Build(typeof(HandlerWithMultipleOptions));

        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void Build_SingleProperty_WithOption_ReturnsOption()
    {
        PropertyInfo prop = typeof(HandlerWithOption).GetProperty(nameof(HandlerWithOption.Verbose))!;

        Option? result = OptionManager.Build(prop);

        Assert.NotNull(result);
        Assert.Equal("--verbose", result.Name);
    }

    [Fact]
    public void Build_SingleProperty_WithoutOption_ReturnsNull()
    {
        PropertyInfo prop = typeof(HandlerWithNoOptions).GetProperty(nameof(HandlerWithNoOptions.PlainProperty))!;

        Option? result = OptionManager.Build(prop);

        Assert.Null(result);
    }

    [Fact]
    public void Build_RequiredOption_IsRequired()
    {
        Option[] result = OptionManager.Build(typeof(HandlerWithRequiredOption));

        Assert.Single(result);
        Assert.True(result[0].Required);
    }

    [Fact]
    public void Build_PropertyArray_WithMixedAttributes_ReturnsOnlyOptionProperties()
    {
        PropertyInfo[] props =
        [
            typeof(HandlerWithOption).GetProperty(nameof(HandlerWithOption.Verbose))!,
            typeof(HandlerWithNoOptions).GetProperty(nameof(HandlerWithNoOptions.PlainProperty))!
        ];

        Option[] result = OptionManager.Build(props);

        Assert.Single(result);
    }
}
