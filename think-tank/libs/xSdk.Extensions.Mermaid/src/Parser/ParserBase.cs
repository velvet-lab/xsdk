using System.Collections.Generic;
using xSdk.Extensions.Mermaid.Data;
using xSdk.Extensions.Mermaid.Lexer;

namespace xSdk.Extensions.Mermaid.Parser
{
    internal abstract class ParserBase
    {
        public abstract Diagram Parse(IEnumerable<Token> tokens);
    }
}
