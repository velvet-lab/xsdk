using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugins.WebSecurity
{
    public sealed class WebSecuritySetup : Setup, IWebSecuritySetup
    {
        [Variable(
            name: Definitions.Origins.Name,
            template: Definitions.Origins.Template,
            helpText: Definitions.Origins.HelpText,
            protect: true)]
        public string Origins
        {
            get => ReadValue<string>(Definitions.Origins.Name);
            set => SetValue(Definitions.Origins.Name, value);
        }

        public static class Definitions
        {
            public static class Origins
            {
                public const string Name = "origins";
                public const string Template = "--origins <origins>";
                public const string HelpText = "Comma seperated list of origins";
            }
        }
    }
}
