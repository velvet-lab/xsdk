using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.AI;

[VariablePrefix("ai")]
public sealed class AIPluginOptions : PluginOptions
{
    internal const string DefaultChatClientKey = nameof(DefaultChatClientKey);

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

    [Variable(
        name: Definitions.EmbeddingModel.Name,
        template: Definitions.EmbeddingModel.Template,
        helpText: Definitions.EmbeddingModel.HelpText
    )]
    public string? EmbeddingModel
    {
        get => ReadValue<string>(Definitions.EmbeddingModel.Name);
        set => SetValue(Definitions.EmbeddingModel.Name, value);
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
        public static class Model
        {
            public const string Name = "model";
            public const string Template = "--model <model>";
            public const string HelpText = "The default model to use for the chat client";
        }

        public static class EmbeddingModel
        {
            public const string Name = "embeddingmodel";
            public const string Template = "--embeddingmodel <embeddingmodel>";
            public const string HelpText = "The default model to use for the embedding client";
        }

        public static class Path
        {
            public const string Name = "path";
            public const string Template = "--path <path>";
            public const string HelpText = "The YAML definitions path for the AI agents and workflows";
        }
    }
}
