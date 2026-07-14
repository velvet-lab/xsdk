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

namespace xSdk.Extensions.Commands;

public class ReplConsoleBuilderExtensionTests
{
    [Fact]
    public void WithBanner_SetsBannerAction()
    {
        var builder = new ReplConsoleBuilder();
        bool called = false;
        Action banner = () => called = true;

        var returned = builder.WithBanner(banner);

        Assert.Same(builder, returned);
        builder.CreateBannerAction?.Invoke();
        Assert.True(called);
    }

    [Fact]
    public void WithLastWill_SetsLastWillAction()
    {
        var builder = new ReplConsoleBuilder();
        bool called = false;

        var returned = builder.WithLastWill(() => called = true);

        Assert.Same(builder, returned);
        builder.CreateLastWillAction?.Invoke();
        Assert.True(called);
    }

    [Fact]
    public void WithUserPrompt_SetsUserPromptAction()
    {
        var builder = new ReplConsoleBuilder();
        Func<string> prompt = () => "user> ";

        var returned = builder.WithUserPrompt(prompt);

        Assert.Same(builder, returned);
        string result = builder.CreateUserPromptAction?.Invoke() ?? string.Empty;
        Assert.Equal("user> ", result);
    }

    [Fact]
    public void WithCustomHelp_SetsHelpAction()
    {
        var builder = new ReplConsoleBuilder();
        IList<Command>? captured = null;
        Action<IList<Command>> help = cmds => captured = cmds;

        var returned = builder.WithCustomHelp(help);

        Assert.Same(builder, returned);
        builder.CreateHelpAction?.Invoke([]);
        Assert.NotNull(captured);
    }
}

public class CommandHandlerBuilderTests
{
    [Fact]
    public void AddCommand_ReturnsNewBuilder()
    {
        var consoleBuilder = new ConsoleBuilder();
        var builder = new CommandHandlerBuilder(consoleBuilder);

        var child = builder.AddCommand<ExitCommand>("exit", "Exit the application");

        Assert.NotNull(child);
        Assert.Equal("exit", child.Name);
    }

    [Fact]
    public void Name_CanBeSetAndRead()
    {
        var consoleBuilder = new ConsoleBuilder();
        var builder = new CommandHandlerBuilder(consoleBuilder)
        {
            Name = "my-command",
            Description = "A test command"
        };

        Assert.Equal("my-command", builder.Name);
        Assert.Equal("A test command", builder.Description);
    }

    [Fact]
    public void Childs_InitiallyEmpty()
    {
        var consoleBuilder = new ConsoleBuilder();
        var builder = new CommandHandlerBuilder(consoleBuilder);

        Assert.Empty(builder.Childs);
    }

    [Fact]
    public void Command_BeforeBuild_ThrowsInvalidOperationException()
    {
        var consoleBuilder = new ConsoleBuilder();
        var builder = new CommandHandlerBuilder(consoleBuilder) { Name = "cmd" };

        Assert.Throws<InvalidOperationException>(() => _ = builder.Command);
    }
}
