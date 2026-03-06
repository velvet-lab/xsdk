using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Mermaid.Data;
using xSdk.Extensions.Mermaid.Lexer;

namespace xSdk.Extensions.Mermaid.Parser
{
    internal sealed partial class StateDiagramParser : ParserBase
    {
        private readonly ILogger<StateDiagramParser> _logger;

        public StateDiagramParser(ILogger<StateDiagramParser> logger)
        {
            Descriptions = new List<Description>();
            Transitions = new List<Transition>();
            Compositions = new List<Composition>();

            Choices = new List<Choice>();
            Forks = new List<Fork>();
            Joins = new List<Join>();

            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Diagram Parse(IEnumerable<Token> tokens)
        {
            ParseInternal(tokens);

            ValidateParseResults();

            var states = BuildObjectModel();

            var diagram = new StateDiagram
            {
                Title = Title,
                Version = Version,
                States = states,
            };

            return diagram;
        }
    }
}
