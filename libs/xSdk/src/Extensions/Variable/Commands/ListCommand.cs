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
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace xSdk.Extensions.Variable.Commands;

[Description(Definitions.HelpText)]
internal class ListCommand(IVariableService variableSvc, ILogger<ListCommand> logger) : Command<ListCommandSettings>
{
    internal static class Definitions
    {
        public const string Name = "list";
        public const string HelpText = "Lists all available automation variables";
    }

    protected override int Execute(CommandContext context, ListCommandSettings settings, CancellationToken cancellationToken)
    {
        var format = settings.FormatString;
        if (settings.ShowHelp)
        {
            format = "Name, Help";
        }
        PrintAsTable(format);

        return 0;
    }

    private void PrintAsTable(string format = default)
    {
        logger.LogInformation("Print variables as table");

        var fields = format.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        logger.LogTrace("Prepare data");
        var items = variableSvc
            .Variables.Where(x => !x.IsHidden)
            .Select(x =>
            {
                var value = variableSvc.ReadVariableValue<object>(x.Name);
                var valueAsString = string.Empty;
                if (value != null)
                {
                    valueAsString = value.ToString();
                }

                return new
                {
                    Name = x.Name,
                    Template = x.Template,
                    Prefix = x.Prefix,
                    Defined = variableSvc.ExistsVariable(x.Name).ToString(),
                    Protected = x.IsProtected.ToString(),
                    Value = valueAsString,
                    Help = x.HelpText,
                };
            })
            .OrderBy(x => x.Prefix)
            .OrderBy(x => x.Template);

        logger.LogTrace("Create table");
        var table = new Table().Border(TableBorder.MinimalHeavyHead).Title("Overview of loaded Variable");

        logger.LogTrace("Add header columns");
        foreach (var field in fields)
        {
            table.AddColumn(field);
        }

        logger.LogTrace("Add variable rows");
        foreach (var item in items)
        {
            var itemType = item.GetType();
            var rowItems = new List<string>();
            foreach (var field in fields)
            {
                var property = itemType.GetProperty(field);
                var value = property.GetValue(item, null);
                if (value != null)
                {
                    rowItems.Add(value.ToString());
                }
                else
                {
                    rowItems.Add("");
                }
            }
            table.AddRow(rowItems.ToArray());
        }

        logger.LogTrace("Write table");
        AnsiConsole.Write(table);
    }

    
}
