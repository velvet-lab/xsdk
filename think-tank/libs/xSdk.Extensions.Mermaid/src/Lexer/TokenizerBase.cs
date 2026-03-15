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
using System.Linq;

namespace xSdk.Extensions.Mermaid.Lexer
{
    internal abstract class TokenizerBase
    {
        public IEnumerable<Token> Tokenize(string content)
        {
            var tokens = TokenizeInternal(content).ToList();
            PostProcess(tokens);

            return tokens;
        }

        private IEnumerable<Token> TokenizeInternal(string content)
        {
            var tokenMatches = FindTokenMatches(content);

            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex).OrderBy(x => x.Key).ToList();

            TokenMatch lastMatch = null;
            for (int i = 0; i < groupedByIndex.Count; i++)
            {
                var bestMatch = groupedByIndex[i].OrderBy(x => x.Weight).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                    continue;

                yield return new Token { Definition = bestMatch.Definition, Value = bestMatch.Value };

                lastMatch = bestMatch;
            }

            yield return new Token { Definition = Terminator };
        }

        private IEnumerable<TokenMatch> FindTokenMatches(string content)
        {
            var result = new List<TokenMatch>();

            foreach (var tokenDefinition in TokenDefinitions)
            {
                result.AddRange(tokenDefinition.FindMatches(content));
            }

            return result;
        }

        protected abstract IEnumerable<TokenDefinition> TokenDefinitions { get; }

        protected abstract TokenDefinition Terminator { get; }

        protected virtual void PostProcess(IEnumerable<Token> tokens) { }
    }
}
