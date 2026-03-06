using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace xSdk.Extensions.Mermaid.Lexer
{
    internal static class TokenDefinitionExtensions
    {
        public static IEnumerable<TokenMatch> FindMatches(this TokenDefinition definition, string inputString)
        {
            var regex = new Regex(definition.RegEx, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var matches = regex.Matches(inputString);
            for (int i = 0; i < matches.Count; i++)
            {
                yield return new TokenMatch
                {
                    StartIndex = matches[i].Index,
                    EndIndex = matches[i].Index + matches[i].Length,
                    Definition = definition,
                    Value = matches[i].Value.Trim(),
                };
            }
        }
    }
}
