using System;

namespace xSdk.Extensions.Mermaid.Lexer
{
    public sealed class Token
    {
        public string Value { get; set; }

        public TokenDefinition Definition { get; set; }

        public override string ToString() => Definition.ToString();
    }
}
