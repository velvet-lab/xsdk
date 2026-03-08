using System.Text.Json.Serialization;
using NLog;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Data;

[VariablePrefix("Vault")]
public class VaultSetup : Setup
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    [Variable(
       name: Definitions.Endpoint.Name,
       template: Definitions.Endpoint.Template,
       helpText: Definitions.Endpoint.HelpText,
       hidden: true)]
    [JsonPropertyName(Definitions.Endpoint.Name)]
    public string Endpoint
    {
        get => ReadValue<string>(Definitions.Endpoint.Name);
        set => SetValue(Definitions.Endpoint.Name, value);
    }

    protected override void ValidateSetup()
    {
        this.ValidateMember(x => string.IsNullOrEmpty(x.Endpoint), "Vault endpoint is missing", Definitions.Endpoint.Name);
    }

    private class Definitions
    {
        public static class Endpoint
        {
            public const string Name = "endpoint";
            public const string Template = $"--vault-endpoint <endpoint>";
            public const string HelpText = "Endpoint where hashicorp vault lives";
        }
    }
}
