using System;
using System.Collections.Generic;
using System.Text;
using Bogus.DataSets;

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
