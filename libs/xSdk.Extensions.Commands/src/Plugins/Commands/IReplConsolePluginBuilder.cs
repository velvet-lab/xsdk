using System.CommandLine;

namespace xSdk.Plugins.Commands;

public interface IReplConsolePluginBuilder : IConsolePluginBuilder
{
    void CreateBanner();

    void CreateHelp(IList<Command> commands);

    void CreateLastWill();

    string CreateUserPrompt();
}
