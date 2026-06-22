namespace xSdk.Extensions.Commands;

internal class SpecificCommandlineParser : CommandlineParser
{
    private readonly List<string> _defaultArgs = new();

    internal SpecificCommandlineParser(string? args) : base(args)
    {
        BackupDefaultArgs();
    }

    internal static SpecificCommandlineParser Create(string[] args)
        => new(string.Join(" ", args));

    internal SpecificCommandlineParser Reparse(string? args)
    {
        Arguments = ParseInternal(args);
        RestoreDefaultArgs();

        return this;
    }

    private void BackupDefaultArgs()
    {
        _defaultArgs.Clear();
        ExtractArgs(DefaultCommandSettings.Definitions.ContentRoot.Name);
        ExtractArgs(DefaultCommandSettings.Definitions.Stage.Name);
        ExtractArgs(DefaultCommandSettings.Definitions.Demo.Name);
    }

    private void RestoreDefaultArgs()
    {
        var args = _defaultArgs.ToArray();
        if (args.Length > 0)
        {
            List<string> result = [.. Arguments];
            var parser = Parse(args);

            foreach (string arg in args)
            {
                if (!ContainsPattern(arg) && IsPattern(arg))
                {
                    string? value = parser.ReadPattern(arg);
                    result.Add(arg);
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            Arguments = [.. result];
        }
    }

    private void ExtractArgs(string pattern)
    {
        // Remove not needed Commandline Params
        if (ContainsPattern(pattern))
        {
            string? template = Arguments.SingleOrDefault(x => x.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) > -1);
            if (!string.IsNullOrEmpty(template))
            {
                string? value = ReadPattern(pattern);
                _defaultArgs.Add(template);
                if (!string.IsNullOrEmpty(value))
                {
                    _defaultArgs.Add(value);
                }
            }
        }
    }
}
