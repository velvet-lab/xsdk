using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace xSdk.Extensions.Commands;

internal class SilentHelpProvider : IHelpProvider
{
    public IEnumerable<IRenderable> Write(ICommandModel model, ICommandInfo? command)
    {
        return Array.Empty<IRenderable>();
    }
}
