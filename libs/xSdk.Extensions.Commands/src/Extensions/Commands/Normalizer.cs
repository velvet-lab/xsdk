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

namespace xSdk.Extensions.Commands;

internal static class Normalizer
{
    private const string LongOptionPrefix = "--";
    private const string ShortOptionPrefix = "-";

    internal static string NormalizeOptionName(string name)
    {
        if (!string.IsNullOrEmpty(name) && !name.StartsWith(LongOptionPrefix))
        {
            return LongOptionPrefix + name;
        }
        return name;
    }

    internal static string[] NormalizeOptionAliases(string[] aliases)
    {
        var normalizedAliases = new List<string>();
        foreach (var alias in aliases)
        {
            if (!string.IsNullOrEmpty(alias))
            {
                if (!alias.StartsWith(ShortOptionPrefix))
                {
                    normalizedAliases.Add(ShortOptionPrefix + alias);
                }
                else
                {
                    normalizedAliases.Add(alias);
                }
            }
        }
        return normalizedAliases.ToArray();
    }
}
