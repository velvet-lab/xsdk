using Microsoft.Agents.ObjectModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace xSdk.Extensions.AI;

public class AIDefinition
{
    public string? Name => Metadata?.Name;

    public string? Description => Metadata?.Description;

    public string? Instructions => Metadata?.Instructions?.ToTemplateString();

    public string? Model => Metadata?.Model?.ExtensionData?.ReadValue("name");

    public GptComponentMetadata? Metadata { get; internal set; }

    public string? FilePath { get; internal set; }

    public IEnumerable<AIFunction> LoadTools(IServiceProvider provider)
    {
        // Nur Tools injizieren, die im YAML deklariert sind
        IEnumerable<string?>? toolNames = Metadata?.Tools.Select(x => x.GetType().GetProperty("Name")?.GetValue(x) as string);
        if (toolNames is not null && toolNames.Any())
        {
            foreach (string? toolName in toolNames.Where(toolName => !string.IsNullOrEmpty(toolName)))
            {
                AIFunction? tool = provider.GetKeyedService<AIFunction>(toolName);
                if (tool is not null)
                {
                    yield return tool;
                }
            }
        }
    }
}
