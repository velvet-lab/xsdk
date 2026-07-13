using xSdk.Extensions.Options;
using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugins.AI;

[VariablePrefix("ai")]
public sealed class AIOptions : OptionsBase
{
    internal const string DefaultChatClientKey = nameof(DefaultChatClientKey);

    [Variable(
        name: Definitions.IsDevUIEnabled.Name,
        template: Definitions.IsDevUIEnabled.Template,
        helpText: Definitions.IsDevUIEnabled.HelpText
    )]
    public bool IsDevUiEnabled
    {
        get => ReadValue<bool>(Definitions.IsDevUIEnabled.Name);
        set => SetValue(Definitions.IsDevUIEnabled.Name, value);
    }

    [Variable(
        name: Definitions.IsTelemetryEnabled.Name,
        template: Definitions.IsTelemetryEnabled.Template,
        helpText: Definitions.IsTelemetryEnabled.HelpText
    )]
    public bool IsTelemetryEnabled
    {
        get => ReadValue<bool>(Definitions.IsTelemetryEnabled.Name);
        set => SetValue(Definitions.IsTelemetryEnabled.Name, value);
    }

    [Variable(
        name: Definitions.IsSensitiveDataEnabled.Name,
        template: Definitions.IsSensitiveDataEnabled.Template,
        helpText: Definitions.IsSensitiveDataEnabled.HelpText
    )]
    public bool IsSensitiveDataEnabled
    {
        get => ReadValue<bool>(Definitions.IsSensitiveDataEnabled.Name);
        set => SetValue(Definitions.IsSensitiveDataEnabled.Name, value);
    }

    [Variable(
        name: Definitions.ExposeOpenAIEndpoints.Name,
        template: Definitions.ExposeOpenAIEndpoints.Template,
        helpText: Definitions.ExposeOpenAIEndpoints.HelpText
    )]
    public bool ExposeOpenAIEndpoints
    {
        get => ReadValue<bool>(Definitions.ExposeOpenAIEndpoints.Name);
        set => SetValue(Definitions.ExposeOpenAIEndpoints.Name, value);
    }

    [Variable(
        name: Definitions.Path.Name,
        template: Definitions.Path.Template,
        helpText: Definitions.Path.HelpText
    )]
    public string? Path
    {
        get => ReadValue<string>(Definitions.Path.Name);
        set => SetValue(Definitions.Path.Name, value);
    }

    public static class Definitions
    {
        //public static class Model
        //{
        //    public const string Name = nameof(Model);
        //    public const string Template = "--model <model>";
        //    public const string HelpText = "The default model to use for the chat client";
        //}

        //public static class EmbeddingModel
        //{
        //    public const string Name = nameof(EmbeddingModel);
        //    public const string Template = "--embeddingmodel <embeddingmodel>";
        //    public const string HelpText = "The default model to use for the embedding client";
        //}

        public static class IsTelemetryEnabled
        {
            public const string Name = "enable_telemetry";
            public const string Template = "--enable-telemetry";
            public const string HelpText = "Enable telemetry for the AI agents and workflows";
        }

        public static class IsSensitiveDataEnabled
        {
            public const string Name = "enable_sensitive_data";
            public const string Template = "--enable-sensitive-data";
            public const string HelpText = "Enable sensitive data for the AI agents and workflows";
        }

        public static class ExposeOpenAIEndpoints
        {
            public const string Name = "expose_openai_endpoints";
            public const string Template = "--expose-openai-endpoints";
            public const string HelpText = "Expose the OpenAI endpoints for the AI agents and workflows";
        }

        public static class IsDevUIEnabled
        {
            public const string Name = "enable_devui";
            public const string Template = "--enable-devui";
            public const string HelpText = "Enable the Dev UI for the AI agents and workflows";
        }

        public static class Path
        {
            public const string Name = nameof(Path);
            public const string Template = "--path <path>";
            public const string HelpText = "The YAML definitions path for the AI agents and workflows";
        }
    }
}
