using System.Collections.Generic;

namespace xSdk.Extensions.Mermaid.Lexer
{
    public interface ILexerService
    {
        IEnumerable<Token> Tokenize(string content, DiagramType diagramType);
    }
}
