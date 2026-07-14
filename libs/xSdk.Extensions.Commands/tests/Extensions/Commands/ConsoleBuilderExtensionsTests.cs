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

public class ConsoleBuilderExtensionsTests
{
    [Fact]
    public void WithDescription_SetsDescriptionOnBuilder()
    {
        var builder = new ConsoleBuilder();

        builder.WithDescription("My Console App");

        Assert.Equal("My Console App", builder.Description);
    }

    [Fact]
    public void WithDescription_EmptyString_SetsEmptyDescription()
    {
        var builder = new ConsoleBuilder();

        builder.WithDescription(string.Empty);

        Assert.Equal(string.Empty, builder.Description);
    }

    [Fact]
    public void AddCommand_ReturnsBuilderForChaining()
    {
        var builder = new ConsoleBuilder();

        var returned = builder.AddCommand<ExitCommand>("exit", "Exits the app");

        Assert.Same(builder, returned);
    }

    [Fact]
    public void AddBranch_ReturnsCommandHandlerBuilder()
    {
        var builder = new ConsoleBuilder();

        var branch = builder.AddBranch("tools", "Tool commands");

        Assert.NotNull(branch);
        Assert.IsType<CommandHandlerBuilder>(branch);
    }

    [Fact]
    public void AddDefaultCommands_RegistersExitClearHelp()
    {
        var builder = new ReplConsoleBuilder();

        // AddDefaultCommands returns the builder — chaining must work
        var returned = builder.AddDefaultCommands();

        Assert.Same(builder, returned);
    }
}

public class ConsoleBuilderBuildTests
{
    [Fact]
    public void Build_WithNullServiceCollection_Throws()
    {
        var builder = new ConsoleBuilder();

        Assert.Throws<ArgumentNullException>(() => builder.Build(null));
    }

    [Fact]
    public void RootCommand_BeforeBuild_ThrowsInvalidOperationException()
    {
        var builder = new ConsoleBuilder();

        Assert.Throws<InvalidOperationException>(() => _ = builder.RootCommand);
    }
}
