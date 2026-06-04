using xSdk.Extensions.AI;

namespace xSdk.Demos;

public static class OllamaConfiguration
{
    public static void Default(AIPluginOptions options)
    {
        // options.Model = "phi4-mini";
        options.Model = "qwen3:8b";
        options.EmbeddingModel = "qwen3-embedding:8b";
        options.Path = Environment.CurrentDirectory;
    }
}
