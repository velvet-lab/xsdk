using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Mermaid.Data;
using xSdk.Extensions.Mermaid.Lexer;

namespace xSdk.Extensions.Mermaid.Parser
{
    internal class ParserService : IParserService
    {
        private readonly StateDiagramParser _stateDiagramParser;
        private readonly ILogger<ParserService> _logger;

        public ParserService(StateDiagramParser stateDiagramParser, ILogger<ParserService> logger)
        {
            _stateDiagramParser = stateDiagramParser ?? throw new System.ArgumentNullException(nameof(stateDiagramParser));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public Diagram Parse(IEnumerable<Token> tokens, DiagramType diagramType)
        {
            if (diagramType == DiagramType.StateDiagram)
            {
                _logger.LogInformation("Parse state diagram tokens");
                return _stateDiagramParser.Parse(tokens);
            }

            return null;
        }
    }
}
