using Microsoft.Agents.ObjectModel;
using Microsoft.Agents.ObjectModel.Yaml;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.AI;

internal static class YamlDeclarationTool
{
    extension(AIDefinition definition)
    {
        internal bool TryReadYamlContent(AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions, out string? content)
        {
            string? basePath = pluginOptions?.Path;
            if (environmentOptions is not null && string.IsNullOrEmpty(basePath))
            {
                basePath = environmentOptions.ContentRoot;
            }

            if (string.IsNullOrEmpty(basePath))
            {
                basePath = Environment.CurrentDirectory;
            }

            string filePath = Path.Join(basePath, definition.FilePath);
            if (File.Exists(filePath))
            {
                content = File.ReadAllText(filePath);
                return true;
            }

            content = default;
            return false;
        }

        internal bool TryReadMetadata(AIPluginOptions? pluginOptions, EnvironmentOptions? environmentOptions, out GptComponentMetadata? metadata)
        {
            if (!definition.TryReadYamlContent(pluginOptions, environmentOptions, out string? content))
            {
                metadata = default;
                return false;
            }

            if (string.IsNullOrEmpty(content))
            {
                metadata = default;
                return false;
            }

            using var yamlReader = new StringReader(content);
            BotElement rootElement = YamlSerializer.Deserialize<BotElement>(yamlReader) ?? throw new InvalidDataException("Text does not contain a valid agent definition.");

            if (rootElement is not GptComponentMetadata promptAgent)
            {
                throw new InvalidDataException($"Unsupported root element: {rootElement.GetType().Name}. Expected an {nameof(GptComponentMetadata)}.");
            }

            metadata = promptAgent;
            return true;
        }
    }
}
