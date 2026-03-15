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
