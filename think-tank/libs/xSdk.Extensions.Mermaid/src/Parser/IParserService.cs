using System.Collections.Generic;
using xSdk.Extensions.Mermaid.Data;
using xSdk.Extensions.Mermaid.Lexer;

namespace xSdk.Extensions.Mermaid.Parser
{
    public interface IParserService
    {
        Diagram Parse(IEnumerable<Token> tokens, DiagramType diagramType);
    }
}
