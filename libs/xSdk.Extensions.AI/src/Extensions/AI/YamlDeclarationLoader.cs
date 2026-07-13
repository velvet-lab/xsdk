using System.Diagnostics;
using System.Reflection;
using CommunityToolkit.Diagnostics;
using Microsoft.Agents.ObjectModel;
using Microsoft.Agents.ObjectModel.Yaml;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Plugins.AI;
using xSdk.Tools;

namespace xSdk.Extensions.AI;

public sealed class YamlDeclarationLoader(IOptions<AIOptions> aiOptions, IOptions<EnvironmentOptions> environmentOptions)
{
    public GptComponentMetadata? FromInstructions(string agentName, string instructions, string model)
    {
        var prompt = @$"
kind: Prompt
name: {agentName}
instructions: {instructions}
model:
  name: {model}
";
        return LoadMetadata(prompt);
    }

    public GptComponentMetadata? FromFile(string filePath)
    {       
        string? basePath = aiOptions.Value.Path;
        if (environmentOptions is not null && string.IsNullOrEmpty(basePath))
        {
            basePath = environmentOptions.Value.ContentRoot;
        }

        if (string.IsNullOrEmpty(basePath) || Debugger.IsAttached)
        {
            basePath = Environment.CurrentDirectory;
        }

        filePath = Path.Join(basePath, filePath);
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            return LoadMetadata(content);
        }

        return default;
    }

    public static GptComponentMetadata? FromEmbeddedResource(Assembly assembly, string @namespace, string filePath)
    {
        EmbeddedResourceLoader resourceLoader = new EmbeddedResourceLoader(assembly, @namespace);
        if(resourceLoader.TryReadResource(filePath, out string? content))
        {
            if (!string.IsNullOrEmpty(content))
            {
                return LoadMetadata(content);
            }
        }
        return default;
    }

    private static GptComponentMetadata? LoadMetadata(string content)
    {
        Guard.IsNotNullOrEmpty(content);

        using var yamlReader = new StringReader(content);
        BotElement rootElement = YamlSerializer.Deserialize<BotElement>(yamlReader) ?? throw new InvalidDataException("Text does not contain a valid agent definition.");

        if (rootElement is not GptComponentMetadata promptAgent)
        {
            throw new InvalidDataException($"Unsupported root element: {rootElement.GetType().Name}. Expected an {nameof(GptComponentMetadata)}.");
        }

        return promptAgent;
    }    
}
