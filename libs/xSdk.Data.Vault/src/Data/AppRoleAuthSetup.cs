using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;
using NLog;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    [VariablePrefix("AppRoleAuth")]
    public class AppRoleAuthSetup : Setup
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        [Variable(
            name: Definitions.RoleId.Name,
            template: Definitions.RoleId.Template,
            helpText: Definitions.RoleId.HelpText,
            hidden: true)]
        [JsonPropertyName(Definitions.RoleId.Name)]
        public string RoleId
        {
            get => ReadValue<string>(Definitions.RoleId.Name);
            set => SetValue(Definitions.RoleId.Name, value);
        }

        [Variable(
            name: Definitions.Secret.Name,
            template: Definitions.Secret.Template,
            helpText: Definitions.Secret.HelpText,
            hidden: true)]
        [JsonPropertyName(Definitions.Secret.Name)]
        public string Secret
        {
            get => ReadValue<string>(Definitions.Secret.Name);
            set => SetValue(Definitions.Secret.Name, value);
        }

        protected override void ValidateSetup()
        {
            this.ValidateMember(x => string.IsNullOrEmpty(x.RoleId), "RoleId for vault approle auth is missing", Definitions.RoleId.Name);
            this.ValidateMember(x => string.IsNullOrEmpty(x.Secret), "Secret for vault approle auth is missing", Definitions.RoleId.Name);
        }

        private class Definitions
        {
            public static class RoleId
            {
                public const string Name = "role_id";
                public const string Template = $"--vault-role-id <role>";
                public const string HelpText = "RoleId for approle based auth to access vault";
            }

            public static class Secret
            {
                public const string Name = "role_secret";
                public const string Template = $"--vault-role-secret <secret>";
                public const string HelpText = "Secret for approle based auth to access vault";
            }
        }
    }
}
