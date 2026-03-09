using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Demos;

[VariablePrefix("custom")]
public sealed class SetupWithPrefix : Setup
{
    [Variable(
        name: Definitions.Prop1.Name,
        template: Definitions.Prop1.Template,
        protect: true)]
    public string WithAppPrefix_WithSetupPrefix
    {
        get => ReadValue<string>(Definitions.Prop1.Name);
        set => SetValue(Definitions.Prop1.Name, value);
    }

    [Variable(
        name: Definitions.Prop2.Name,
        template: Definitions.Prop2.Template,
        protect: true,
        noPrefix: true)]
    public string NoAppPrefix_NoSetupPrefix
    {
        get => ReadValue<string>(Definitions.Prop2.Name);
        set => SetValue(Definitions.Prop2.Name, value);
    }

    internal static class Definitions
    {
        public static class Prop1
        {
            public const string Name = "with-app-prefix-with-setup-prefix";
            public const string Template = $"--with-app-prefix-with-setup-prefix <services>";
        }

        public static class Prop2
        {
            public const string Name = "no-app-prefix-no-setup-prefix";
            public const string Template = $"--no-app-prefix-no-setup-prefix <services>";
        }
    }
}
