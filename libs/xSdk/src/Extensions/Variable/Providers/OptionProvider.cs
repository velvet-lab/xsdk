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

using Microsoft.Extensions.Configuration;
using xSdk.Extensions.Options;

namespace xSdk.Extensions.Variable.Providers;

internal class OptionProvider(IConfiguration configuration, ApplicationOptions options) : VariableProviderBase
{
    protected override bool ExistsVariable(IVariable variable)
    {
        var result = ReadVariable(variable);

        return result != null;
    }

    protected override object? ReadVariable(IVariable variable)
    {
        if (configuration != null && variable != null)
        {
            var mainSection = configuration.GetSection(options.Prefix.ToLower());
            if (mainSection != null)
            {
                var sectionName = variable.Name;
                if (!string.IsNullOrEmpty(variable.Prefix))
                    sectionName = variable.Prefix;

                try
                {
                    var result = ReadValue(mainSection, variable);
                    if (result == null)
                    {
                        var section = mainSection.GetSection(NormalizeName(variable.Prefix, sectionName, true));
                        if (section != null)
                        {
                            result = ReadValue(section, variable);
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return default;
    }

    private string NormalizeName(string? prefix, string name, bool isSection)
    {
        if (!isSection)
        {
            if (!string.IsNullOrEmpty(prefix) && name.StartsWith(prefix))
            {
                name = name.Substring(prefix.Length);
            }
        }

        if (name.IndexOf("-") > -1)
            name = name.Replace("-", "");

        return name.Trim();
    }

    private object? ReadValue(IConfigurationSection section, IVariable variable) =>
        section.GetValue(variable.ValueType, NormalizeName(variable.Prefix, variable.Name, false));
}
