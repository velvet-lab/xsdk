using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.AI;

public sealed class AIPluginOptions : PluginOptions
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

    [Variable(
        name: Definitions.Model.Name,
        template: Definitions.Model.Template,
        helpText: Definitions.Model.HelpText
    )]
    public string? Model
    {
        get => ReadValue<string>(Definitions.Model.Name);
        set => SetValue(Definitions.Model.Name, value);
    }

    public static class Definitions
    {
        public static class Endpoint
        {
            public const string Name = "ai-endpoint";
            public const string Template = "--endpoint <endpoint>";
            public const string HelpText = "The endpoint for OpenAI API";
            public const string DefaultValue = "https://api.openai.com/v1";
        }

        public static class ApiKey
        {
            public const string Name = "ai-apikey";
            public const string Template = "--apikey <apikey>";
            public const string HelpText = "The API key for OpenAI API";            
        }

        public static class Model
        {
            public const string Name = "ai-model";
            public const string Template = "--model <model>";
            public const string HelpText = "The model to use for the chat client";
        }
    }
}
