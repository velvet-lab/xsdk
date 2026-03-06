using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Variable.Fakes
{
    [VariableNoPrefix()]
    internal class SetupWithNoPrefix : Setup
    {
        [Variable(
            name: Definitions.StringValue.Name,
            template: Definitions.StringValue.Template,
            helpText: Definitions.StringValue.HelpText,
            defaultValue: Definitions.StringValue.DefaultValue,
            resourceNames: new[] { "xsdk.environment.stage", "deployment.environment" }
        )]
        public string StringValue
        {
            get => this.ReadValue<string>(Definitions.StringValue.Name);
            set => this.SetValue(Definitions.StringValue.Name, value);
        }

        public static class Definitions
        {
            public static class StringValue
            {
                public const string Name = "stringValue";
                public const string Template = "--string-value <value>";
                public const string HelpText = "StringValue for use.";
                public const string DefaultValue = "MyDefaultValue";
            }
        }
    }
}
