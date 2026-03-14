using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugins.Documentation;

[VariablePrefix("openapi")]
public sealed class DocumentationSetup : Setup, IDocumentationSetup
{
    [Variable(
        name: Definitions.DocumentPattern.Name,
        template: Definitions.DocumentPattern.Template,
        helpText: Definitions.DocumentPattern.HelpText,
        defaultValue: Definitions.DocumentPattern.DefaultValue
    )]
    public string DocumentPattern
    {
        get => ReadValue<string>(Definitions.DocumentPattern.Name);
        set => SetValue(Definitions.DocumentPattern.Name, value);
    }

    [Variable(
        name: Definitions.Enabled.Name,
        template: Definitions.Enabled.Template,
        helpText: Definitions.Enabled.HelpText
    )]
    public bool Enabled
    {
        get => ReadValue<bool>(Definitions.Enabled.Name);
        set => SetValue(Definitions.Enabled.Name, value);
    }

    public static class Definitions
    {
        public static class Enabled
        {
            public const string Name = "enabled";
            public const string Template = "--enabled";
            public const string HelpText = "Enabled OpenAPI document generation and UI";
        }

        public static class DocumentPattern
        {
            public const string Name = "document-pattern";
            public const string Template = "--document-pattern <pattern>";
            public const string HelpText = "DocumentPattern prefix for the api";
            public const string DefaultValue = "openapi/{documentName}.json";
        }
    }
}
