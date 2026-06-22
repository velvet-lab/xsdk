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

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace xSdk.Extensions.Commands;

public class CommandlineParser
{
    protected CommandlineParser(string? args)
    {
        Arguments = ParseInternal(args);
    }

    public string[] Arguments { get; protected set; }

    public static CommandlineParser Parse()
        => new(Environment.CommandLine);

    public static CommandlineParser Parse(string? input)
        => new(input);

    public static CommandlineParser Parse(string[] input)
        => new(string.Join(" ", input));

    public bool ContainsPattern(string? pattern)
    {
        bool result = false;

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

    public string? ReadPattern(string pattern, string? defaultValue = default)
    {
        var comparer = new PatternComparer();
        for (int i = 0; i < Arguments.Length; i++)
        {
            if (comparer.Equals(Arguments[i], pattern))
            {
                try
                {
                    string currentValue = Arguments[i];
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

    protected string[] ParseInternal(string? input)
    {
        var result = new List<string>();
        if (!string.IsNullOrEmpty(input))
        {
            while (input.IndexOf('\"') > -1)
            {
                input = input.Replace("\"", "");
            }

            while (input.IndexOf('\'') > -1)
            {
                input = input.Replace("'", "");
            }

            string[] splittedCommandline = input.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            if (splittedCommandline.Length == 1 && string.Compare(input, Assembly.GetEntryAssembly()?.Location, true) == 0)
            {
                return [.. result];
            }

            bool optionStarted = false;
            string currentValue = string.Empty;
            foreach (string splitValue in splittedCommandline)
            {
                if ((splitValue.StartsWith('-') || splitValue.StartsWith("--")) && !TestOptionIsNumeric(splitValue))
                {
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        result.Add(CleanCommandArg(currentValue.Trim()));
                    }

                    currentValue = string.Empty;
                    optionStarted = true;

                    (string? optionKey, string? optionValue) = ValidateCommandArg(splitValue.Trim());
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

        return RemoveEntryAssemblyIfExists([.. result]);
    }

    protected static bool IsPattern(string value) => value.StartsWith('-') || value.StartsWith("--");

    private static string CleanCommandArg(string pattern)
    {
        string[] letters = ["\\", "\""];

        foreach (string letter in letters)
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

    private static bool TestOptionIsNumeric(string option)
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

    private static (string, string) ValidateCommandArg(string value)
    {
        string option = value;
        string optionValue = "";

        if (value.IndexOf('=') > -1 || value.IndexOf(':') > -1)
        {
            int equalStart = value.IndexOf('=');
            int colonStart = value.IndexOf(':');

            if (equalStart > -1)
            {
                string[] splittedOptions = value.Split(['='], StringSplitOptions.RemoveEmptyEntries);
                option = splittedOptions[0].Trim();
                optionValue = splittedOptions[1].Trim();
            }

            if (colonStart > -1 && colonStart < equalStart)
            {
                string[] splittedOptions = value.Split([':'], StringSplitOptions.RemoveEmptyEntries);
                option = splittedOptions[0].Trim();
                optionValue = splittedOptions[1].Trim();
            }
        }

        return (option, optionValue);
    }

    private static string[] RemoveEntryAssemblyIfExists(string[] args)
    {
        if (args.Length > 1)
        {
            string firstItem = args[0];
            if (firstItem != null && firstItem == Assembly.GetEntryAssembly()?.Location)
            {
                return [.. args.Skip(1)];
            }
        }

        return args;
    }
}

internal class PatternComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return
            string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0
            || string.Compare(x, $"--{y}", StringComparison.InvariantCultureIgnoreCase) == 0
            || string.Compare(x, $"-{y}", StringComparison.InvariantCultureIgnoreCase) == 0;
    }

    public int GetHashCode([DisallowNull] string obj) => obj.GetHashCode();
}
