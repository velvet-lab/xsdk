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

using xSdk.Extensions.Options;
using xSdk.Tools;

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
        ExtractArgs(EnvironmentOptions.Definitions.ContentRoot.Name);
        ExtractArgs(EnvironmentOptions.Definitions.Stage.Name);
        ExtractArgs(EnvironmentOptions.Definitions.Demo.Name);
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
