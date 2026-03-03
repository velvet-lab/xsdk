namespace xSdk.Extensions.Mermaid.Lexer
{
    internal class TokenMatch
    {
        public TokenDefinition Definition { get; set; }

        public string Value { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public int Weight => Definition.Weight;

        public override string ToString() => Definition.ToString();
    }
}
