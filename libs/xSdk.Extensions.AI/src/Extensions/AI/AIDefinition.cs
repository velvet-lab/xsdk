using Microsoft.Agents.ObjectModel;
using Microsoft.Agents.ObjectModel.Exceptions;

namespace xSdk.Extensions.AI;

public class AIDefinition
{
    public string? Name => Metadata?.Name;

    public string? Description => Metadata?.Description;

    public string? Instructions => Metadata?.Instructions?.ToTemplateString();

    public string? Model => LoadModelName();

    public GptComponentMetadata? Metadata { get; internal set; }

    public string? FilePath { get; internal set; }

    private string? LoadModelName()
    {
        var propertyPath = PropertyPath.Create("name");
        StringDataValue? property = Metadata?.Model?.ExtensionData?.GetProperty<StringDataValue>(propertyPath);

        if (property is not null)
        {
            return property.Value;
        }

        throw new InvalidOperationException("Model information is missing or invalid.");
    }
}
