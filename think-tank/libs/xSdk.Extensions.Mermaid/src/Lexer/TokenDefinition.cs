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

namespace xSdk.Extensions.Mermaid.Lexer
{
    public sealed class TokenDefinition
    {
        public TokenDefinition(string name)
        {
            Name = name;
        }

        public TokenDefinition(string name, string regex)
        {
            Name = name;
            RegEx = regex;
        }

        public TokenDefinition(string name, string regex, int weight)
        {
            Name = name;
            RegEx = regex;
            Weight = weight;
        }

        public string Name { get; }

        public string RegEx { get; }

        public int Weight { get; } = 1;

        public override string ToString() => Name;
    }
}
