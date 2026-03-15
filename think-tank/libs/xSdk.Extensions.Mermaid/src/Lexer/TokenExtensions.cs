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
using System.Linq;

namespace xSdk.Extensions.Mermaid.Lexer
{
    internal static class TokenExtensions
    {
        public static Token Clone(this Token token)
        {
            return new Token { Value = token.Value, Definition = token.Definition };
        }

        public static Token Filter(this IEnumerable<Token> tokens, string name)
        {
            return tokens.FirstOrDefault(x => string.Compare(x.Definition.Name, name, true) == 0);
        }

        public static void PostProcess(this IEnumerable<Token> tokens, string name, Action<Token> tokenAction)
        {
            var token = tokens.Filter(name);
            if (token != null)
            {
                tokenAction(token);
            }
        }
    }
}
