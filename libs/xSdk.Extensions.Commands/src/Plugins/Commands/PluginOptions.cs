using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugins.Commands;

[VariablePrefix("console")]
public class PluginOptions : PluginOptionsBase
{
    [Variable(
            name: Definitions.DisableDefaultHelp.Name,
            template: Definitions.DisableDefaultHelp.Template,
            helpText: Definitions.DisableDefaultHelp.HelpText
        )]
    public bool DisableDefaultHelp
    {
        get => ReadValue<bool>(Definitions.DisableDefaultHelp.Name);
        set => SetValue(Definitions.DisableDefaultHelp.Name, value);
    }

    public static class Definitions
    {
        public static class DisableDefaultHelp
        {
            public const string Name = nameof(DisableDefaultHelp);
            public const string Template = "--disable-help";
            public const string HelpText = "Whether to disable the default help";
        }
    }
}
