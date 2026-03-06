using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugins.Authentication
{
    [VariablePrefix("auth_apikey")]
    public class ApiKeySetup : Setup
    {
        [Variable(
            name: Definitions.Realm.Name,
            template: Definitions.Realm.Template,
            helpText: Definitions.Realm.HelpText,
            defaultValue: Definitions.Realm.DefaultValue
        )]
        public string Realm
        {
            get => ReadValue<string>(Definitions.Realm.Name);
            set => SetValue(Definitions.Realm.Name, value);
        }

        protected override void ValidateSetup()
        {
            this.ValidateMember(x => string.IsNullOrEmpty(x.Realm), "Authentication realm is missing", Definitions.Realm.Name);
        }

        public static class Definitions
        {
            public static class Realm
            {
                public const string Name = "realm";
                public const string Template = "--realm <realm>";
                public const string HelpText = "A realm for Authenication";
                public const string DefaultValue = "xSdk Authentication Realm";
            }
        }
    }
}
