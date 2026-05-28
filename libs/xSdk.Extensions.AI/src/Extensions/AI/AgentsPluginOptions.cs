using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.AI;

public sealed class AgentsPluginOptions : PluginOptions
{
    [Variable(
        name: Definitions.Endpoint.Name,
        template: Definitions.Endpoint.Template,
        helpText: Definitions.Endpoint.HelpText,
        defaultValue: Definitions.Endpoint.DefaultValue
    )]
    public string? Endpoint
    {
        get => ReadValue<string>(Definitions.Endpoint.Name);
        set => SetValue(Definitions.Endpoint.Name, value);
    }

    [Variable(
        name: Definitions.ApiKey.Name,
        template: Definitions.ApiKey.Template,
        helpText: Definitions.ApiKey.HelpText
    )]
    public string? ApiKey
    {
        get => ReadValue<string>(Definitions.ApiKey.Name);
        set => SetValue(Definitions.ApiKey.Name, value);
    }

    public static class Definitions
    {
        public static class Endpoint
        {
            public const string Name = "endpoint";
            public const string Template = "--endpoint <endpoint>";
            public const string HelpText = "The endpoint for OpenAI API";
            public const string DefaultValue = "https://api.openai.com/v1";
        }

        public static class ApiKey
        {
            public const string Name = "apikey";
            public const string Template = "--apikey <apikey>";
            public const string HelpText = "The API key for OpenAI API";            
        }
    }
}
