using System.ComponentModel;
using xSdk.Extensions.Variable;

namespace xSdk.Data.Models;

[Description("Represents a variable")]
public sealed class VariableModel
{
    public VariableModel()
    {

    }

    internal VariableModel(IVariable variable)
    {
        Name = variable.Name;
        HelpText = variable.HelpText;
        Prefix = variable.Prefix;
        IsHidden = variable.IsHidden;
        IsProtected = variable.IsProtected;
        NoPrefix = variable.NoPrefix;
    }

    [Description("The name of the variable")]
    public string Name { get; set; } = string.Empty;

    [Description("The help text for the variable")]
    public string HelpText { get; set; } = string.Empty;

    [Description("Used prefix for the variable")]
    public string Prefix { get; set; } = string.Empty;

    [Description("Is the variable hidden?")]
    public bool IsHidden { get; set; }

    [Description("Is the variable protected?")]
    public bool IsProtected { get; set; }

    [Description("Does the variable have a prefix?")]
    public bool NoPrefix { get; set; }
}
