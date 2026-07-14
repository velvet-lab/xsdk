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

public class ArgumentManagerTests
{
    private sealed class HandlerWithArgument : CommandHandler
    {
        [CommandArgument("file"), Description("The file path")]
        public string? FilePath { get; set; }
    }

    private sealed class HandlerWithNoArguments : CommandHandler
    {
        public string? NoAttributeProperty { get; set; }
    }

    private sealed class HandlerWithMultipleArguments : CommandHandler
    {
        [CommandArgument("source"), Description("Source path")]
        public string? Source { get; set; }

        [CommandArgument("destination"), Description("Destination path")]
        public string? Destination { get; set; }
    }

    [Fact]
    public void Build_TypeWithArgumentProperty_ReturnsArgument()
    {
        Argument[] result = ArgumentManager.Build(typeof(HandlerWithArgument));

        Assert.Single(result);
        Assert.Equal("file", result[0].Name);
    }

    [Fact]
    public void Build_TypeWithNoArgumentAttributes_ReturnsEmpty()
    {
        Argument[] result = ArgumentManager.Build(typeof(HandlerWithNoArguments));

        Assert.Empty(result);
    }

    [Fact]
    public void Build_TypeWithMultipleArguments_ReturnsAll()
    {
        Argument[] result = ArgumentManager.Build(typeof(HandlerWithMultipleArguments));

        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void Build_SingleProperty_WithArgument_ReturnsArgument()
    {
        PropertyInfo prop = typeof(HandlerWithArgument).GetProperty(nameof(HandlerWithArgument.FilePath))!;

        Argument? result = ArgumentManager.Build(prop);

        Assert.NotNull(result);
        Assert.Equal("file", result.Name);
    }

    [Fact]
    public void Build_SingleProperty_WithoutArgument_ReturnsNull()
    {
        PropertyInfo prop = typeof(HandlerWithNoArguments).GetProperty(nameof(HandlerWithNoArguments.NoAttributeProperty))!;

        Argument? result = ArgumentManager.Build(prop);

        Assert.Null(result);
    }

    [Fact]
    public void Build_PropertyArray_WithMixedAttributes_ReturnsOnlyArgumentProperties()
    {
        PropertyInfo[] props =
        [
            typeof(HandlerWithArgument).GetProperty(nameof(HandlerWithArgument.FilePath))!,
            typeof(HandlerWithNoArguments).GetProperty(nameof(HandlerWithNoArguments.NoAttributeProperty))!
        ];

        Argument[] result = ArgumentManager.Build(props);

        Assert.Single(result);
    }
}
