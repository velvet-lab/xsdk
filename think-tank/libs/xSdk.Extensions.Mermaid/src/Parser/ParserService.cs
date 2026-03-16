/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
