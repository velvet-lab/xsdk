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

using Microsoft.Extensions.Logging;
using xSdk.Extensions.Mermaid.Data;
using xSdk.Extensions.Mermaid.Lexer;
using xSdk.Extensions.Mermaid.Parser;

namespace xSdk.Extensions.Mermaid
{
    internal class MermaidService : IMermaidService
    {
        private readonly ILexerService _lexerSvc;
        private readonly IParserService _parserSvc;
        private readonly ILogger<MermaidService> _logger;

        public MermaidService(ILexerService lexerSvc, IParserService parserSvc, ILogger<MermaidService> logger)
        {
            _lexerSvc = lexerSvc ?? throw new ArgumentNullException(nameof(lexerSvc));
            _parserSvc = parserSvc ?? throw new ArgumentNullException(nameof(parserSvc));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Diagram Parse(string content, DiagramType type)
        {
            _logger.LogInformation("Parse content for type '{0}'", type);
            var tokens = _lexerSvc.Tokenize(content, type);
            return _parserSvc.Parse(tokens, type);
        }
    }
}
