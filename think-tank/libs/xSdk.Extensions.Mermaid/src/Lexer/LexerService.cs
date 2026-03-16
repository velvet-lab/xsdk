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
