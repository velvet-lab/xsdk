using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace xSdk.Extensions.Commands;

public partial class CommandlineParser
{
    private CommandlineParser() { }

    public string[] Arguments { get; private set; } = Array.Empty<string>();

    public static CommandlineParser Parse()
    {
        var parser = new CommandlineParser();

        parser.Arguments = parser.ParseInternal(Environment.CommandLine);

        return parser;
    }

    public static CommandlineParser Parse(string? input)
    {
        var parser = new CommandlineParser();

        parser.Arguments = parser.ParseInternal(input);

        return parser;
    }

    public static CommandlineParser Parse(string[] input)
    {
        var parser = new CommandlineParser();

        parser.Arguments = parser.ParseInternal(string.Join(" ", input));

        return parser;
    }

    public CommandlineParser AddDefaultArgs(string[] args)
    {
        if (args != null && args.Length > 0)
        {
            var result = Arguments.ToList();
            var parser = CommandlineParser.Parse(args);
            foreach (var arg in args)
            {
                if (!ContainsPattern(arg) && IsPattern(arg))
                {
                    var value = parser.ReadPattern(arg);
                    result.Add(arg);
                    if (!string.IsNullOrEmpty(value))
                    {
                        result.Add(value);
                    }
                }
            }

            Arguments = result.ToArray();
        }

        return this;
    }

    public bool ContainsPattern(string pattern)
    {
        var result = false;

        if (Arguments != null && !string.IsNullOrEmpty(pattern))
        {
            var comparer = new PatternComparer();
            if (Arguments.Contains(pattern, comparer))
            {
                result = true;
            }
        }
        return result;
    }

    public string ReadPattern(string pattern, string defaultValue = default)
    {
        var comparer = new PatternComparer();
        for (var i = 0; i < Arguments.Length; i++)
        {
            if (comparer.Equals(Arguments[i], pattern))
            {
                try
                {
                    var currentValue = Arguments[i];
                    string? nextValue = null;
                    if ((i + 1) < Arguments.Length)
                    {
                        nextValue = Arguments[i + 1];
                    }

                    if (!string.IsNullOrEmpty(nextValue) && !IsPattern(nextValue))
                    {
                        return nextValue;
                    }
                    else
                    {
                        if (IsPattern(currentValue))
                        {
                            // Its a switch
                            return "true";
                        }
                    }
                }
                catch
                {
                    // Nothing to tell
                }
            }
        }

        return defaultValue;
    }

    private string[] ParseInternal(string? input)
    {
        var result = new List<string>();
        if (!string.IsNullOrEmpty(input))
        {
            while (input.IndexOf("\"") > -1)
            {
                input = input.Replace("\"", "");
            }

            while (input.IndexOf("'") > -1)
            {
                input = input.Replace("'", "");
            }

            var splittedCommandline = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splittedCommandline.Length == 1)
            {
                if (string.Compare(input, Assembly.GetEntryAssembly()?.Location, true) == 0)
                {
                    return result.ToArray();
                }
            }

            var optionStarted = false;
            var currentValue = string.Empty;
            foreach (var splitValue in splittedCommandline)
            {
                if ((splitValue.StartsWith('-') || splitValue.StartsWith("--")) && !TestOptionIsNumeric(splitValue))
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        result.Add(CleanCommandArg(currentValue.Trim()));
                    }

                    currentValue = string.Empty;
                    optionStarted = true;

                    var (optionKey, optionValue) = ValidateCommandArg(splitValue.Trim());
                    result.Add(optionKey);
                    if (!string.IsNullOrEmpty(optionValue))
                    {
                        result.Add(optionValue.Trim());
                    }
                }
                else
                {
                    if (optionStarted)
                    {
                        if (string.IsNullOrEmpty(currentValue))
                        {
                            currentValue += $" {splitValue}";
                        }
                        else
                        {
                            result.Add(CleanCommandArg(currentValue.Trim()));
                            result.Add(splitValue.Trim());
                            currentValue = string.Empty;
                            optionStarted = false;
                        }
                    }
                    else
                    {
                        result.Add(splitValue.Trim());
                    }
                }
            }

            if (!string.IsNullOrEmpty(currentValue))
            {
                result.Add(CleanCommandArg(currentValue.Trim()));
            }
        }

        return RemoveEntryAssemblyIfExists(result.ToArray());
    }

    private string CleanCommandArg(string pattern)
    {
        var letters = new string[] { "\\", "\"" };

        foreach (var letter in letters)
        {
            do
            {
                if (pattern.StartsWith(letter))
                {
                    pattern = pattern.Substring(1);
                }

                if (pattern.EndsWith(letter))
                {
                    pattern = pattern.Substring(0, pattern.Length - 1);
                }
            } while (pattern.StartsWith(letter) || pattern.EndsWith(letter));
        }

        return pattern;
    }

    private bool TestOptionIsNumeric(string option)
    {
        option = CleanCommandArg(option);
        if (short.TryParse(option, out _))
        {
            return true;
        }

        if (int.TryParse(option, out _))
        {
            return true;
        }

        if (long.TryParse(option, out _))
        {
            return true;
        }

        if (ushort.TryParse(option, out _))
        {
            return true;
        }

        if (uint.TryParse(option, out uint _))
        {
            return true;
        }

        if (ulong.TryParse(option, out ulong _))
        {
            return true;
        }

        if (double.TryParse(option, out double _))
        {
            return true;
        }

        if (decimal.TryParse(option, out decimal _))
        {
            return true;
        }

        if (float.TryParse(option, out float _))
        {
            return true;
        }

        return false;
    }

    private (string, string) ValidateCommandArg(string value)
    {
        string option = value;
        string optionValue = "";

        if (value.IndexOf("=") > -1 || value.IndexOf(":") > -1)
        {
            var equalStart = value.IndexOf("=");
            var colonStart = value.IndexOf(":");

            if (equalStart > -1)
            {
                var splittedOptions = value.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                option = splittedOptions[0].Trim();
                optionValue = splittedOptions[1].Trim();
            }

            if (colonStart > -1 && colonStart < equalStart)
            {
                var splittedOptions = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                option = splittedOptions[0].Trim();
                optionValue = splittedOptions[1].Trim();
            }
        }

        return (option, optionValue);
    }

    private string[] RemoveEntryAssemblyIfExists(string[] args)
    {
        if (args.Length > 1)
        {
            var firstItem = args.First();
            if (firstItem != null)
            {
                if (firstItem == Assembly.GetEntryAssembly().Location)
                {
                    return args.Skip(1).ToArray();
                }
            }
        }

        return args;
    }

    public string[] BackupDefaultArgs()
    {
        var result = new List<string>();

        ExtractDefaultArgs(DefaultCommandSettings.Definitions.ContentRoot.Name, ref result);
        ExtractDefaultArgs(DefaultCommandSettings.Definitions.Stage.Name, ref result);
        ExtractDefaultArgs(DefaultCommandSettings.Definitions.Demo.Name, ref result);

        return result.ToArray();
    }

    private void ExtractDefaultArgs(string pattern, ref List<string> result)
    {
        // Remove not needed Commandline Params
        if (ContainsPattern(pattern))
        {
            var template = Arguments.SingleOrDefault(x => x.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) > -1);
            if (!string.IsNullOrEmpty(template))
            {
                var value = ReadPattern(pattern);
                result.Add(template);
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }
            }
        }
    }

    private bool IsPattern(string value)
    {
        return value.StartsWith('-') || value.StartsWith("--");
    }
}

internal class PatternComparer : IEqualityComparer<string>
{
    public bool Equals(string x, string y)
    {
        return (
            string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0
            || string.Compare(x, $"--{y}", StringComparison.InvariantCultureIgnoreCase) == 0
            || string.Compare(x, $"-{y}", StringComparison.InvariantCultureIgnoreCase) == 0
        );
    }

    public int GetHashCode([DisallowNull] string obj)
    {
        return obj.GetHashCode();
    }
}
