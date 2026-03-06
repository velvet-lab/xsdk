using xSdk.Extensions.Mermaid.Data;

namespace xSdk.Extensions.Mermaid
{
    public interface IMermaidService
    {
        Diagram Parse(string content, DiagramType type);
    }
}
