namespace xSdk.Extensions.Commands.Tests.Extensions.Commands;

public class CommandDefinitionsTests
{
    [Fact]
    public void ClearCommand_DefinitionsName_IsClear()
    {
        Assert.Equal("clear", ClearCommand.Definitions.Name);
    }

    [Fact]
    public void ClearCommand_DefinitionsHelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(ClearCommand.Definitions.HelpText));
    }

    [Fact]
    public void ExitCommand_DefinitionsName_IsExit()
    {
        Assert.Equal("exit", ExitCommand.Definitions.Name);
    }

    [Fact]
    public void ExitCommand_DefinitionsHelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(ExitCommand.Definitions.HelpText));
    }

    [Fact]
    public void ConsoleCommand_DefinitionsName_IsConsole()
    {
        Assert.Equal("console", ConsoleCommand.Definitions.Name);
    }

    [Fact]
    public void ConsoleCommand_DefinitionsHelpText_IsNotEmpty()
    {
        Assert.False(string.IsNullOrEmpty(ConsoleCommand.Definitions.HelpText));
    }
}
