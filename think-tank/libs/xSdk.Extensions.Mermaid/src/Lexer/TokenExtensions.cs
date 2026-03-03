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
