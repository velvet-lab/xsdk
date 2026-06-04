using xSdk.Extensions.AI;

namespace xSdk.Demos.AI;

public static class AIPluginHelper
{
    public static void Ollama(AIPluginOptions options)
    {
        options.Endpoint = "http://192.168.189.32:11434/v1";
        options.ApiKey = "sk-none";
        // options.Model = "phi4-mini";
        options.Model = "qwen3:8b";
        options.EmbeddingModel = "qwen3-embedding:8b";
        options.Path = Environment.CurrentDirectory;
    }
}
