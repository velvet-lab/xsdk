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

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DotMake.CommandLine;
using Microsoft.Extensions.Logging;

namespace xSdk.Extensions.Variable.Commands;

[ExcludeFromCodeCoverage(Justification = "Spectre.Console command – renders to an interactive terminal and cannot be unit tested.")]
[Description(Definitions.HelpText)]
[CliCommand(Description = Definitions.HelpText)]
internal class ListCommand(IVariableService variableSvc, ILogger<ListCommand> logger) : Command<ListCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "list";
        public const string HelpText = "Lists all available automation variables";
    }

    protected override int Execute(CommandContext context, ListCommandSettings settings, CancellationToken cancellationToken)
    {
        string? format = settings.FormatString;
        if (settings.ShowHelp)
        {
            format = "Name, Help";
        }

        PrintAsTable(format);

        return 0;
    }

    private void PrintAsTable(string? format)
    {
        logger.LogInformation("Print variables as table");

        if (string.IsNullOrEmpty(format))
        {
            AnsiConsole.WriteLine("No format specified. Please provide a format string to specify which fields to display.");
            return;
        }

        var items = variableSvc
            .Variables.Where(x => !x.IsHidden)
            .Select(x =>
            {
                object? value = variableSvc.ReadVariableValue<object>(x.Name);
                string? valueAsString = string.Empty;
                if (value != null)
                {
                    valueAsString = value.ToString();
                }

                return new
                {
                    x.Name,
                    x.Template,
                    x.Prefix,
                    Defined = variableSvc.ExistsVariable(x.Name).ToString(),
                    Protected = x.IsProtected.ToString(),
                    Value = valueAsString,
                    Help = x.HelpText,
                };
            })
            .OrderBy(x => x.Prefix)
            .ThenBy(x => x.Template);

        logger.LogTrace("Create table");
        Table table = new Table().Border(TableBorder.MinimalHeavyHead).Title("Overview of loaded Variable");

        logger.LogTrace("Add header columns");
        List<string> fields = [.. format.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        foreach (string field in fields)
        {
            table.AddColumn(field);
        }

        logger.LogTrace("Add variable rows");
        foreach (var item in items)
        {
            Type itemType = item.GetType();
            var rowItems = new List<string>();
            foreach (string field in fields)
            {
                PropertyInfo? property = itemType.GetProperty(field);
                if (property != null)
                {
                    object? value = property.GetValue(item, null);
                    if (value != null)
                    {
                        string? valueAsString = value.ToString();
                        if (!string.IsNullOrEmpty(valueAsString))
                        {
                            rowItems.Add(valueAsString);
                        }
                    }
                    else
                    {
                        rowItems.Add("");
                    }
                }
            }

            table.AddRow(rowItems.ToArray());
        }

        logger.LogTrace("Write table");
        AnsiConsole.Write(table);
    }
}
