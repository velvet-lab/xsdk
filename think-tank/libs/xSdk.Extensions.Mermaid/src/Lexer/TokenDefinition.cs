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
