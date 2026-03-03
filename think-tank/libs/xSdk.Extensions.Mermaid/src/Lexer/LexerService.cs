using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Mermaid.Lexer
{
    internal class LexerService : ILexerService
    {
        private readonly ILogger<LexerService> _logger;

        public LexerService(ILogger<LexerService> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Token> Tokenize(string content, DiagramType diagramType)
        {
            if (diagramType == DiagramType.StateDiagram)
            {
                _logger.LogInformation("Tokenzize content for statediagram");
                var tokenizer = new StateDiagramTokenizer();
                return tokenizer.Tokenize(content);
            }

            throw new SdkException("Unknown diagram type given");
        }
    }
}
