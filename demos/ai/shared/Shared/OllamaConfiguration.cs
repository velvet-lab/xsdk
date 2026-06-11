using xSdk.Extensions.AI;

namespace xSdk.Demos;

public static class OllamaConfiguration
{
    public static void Default(AIPluginOptions options)
    {
        // options.Model = "phi4-mini";
        options.Model = "Qwen/Qwen2.5-VL-7B-Instruct-AWQ";
        options.EmbeddingModel = "qwen3-embedding:0.6b";
        options.Path = Environment.CurrentDirectory;
    }
}
