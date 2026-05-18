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
using xSdk.Extensions.Variable;

namespace xSdk.Data.Models;

[Description("Represents a variable")]
public sealed class VariableModel
{
    public VariableModel()
    {

    }

    internal VariableModel(IVariable variable)
    {
        Name = variable.Name;
        HelpText = variable.HelpText;
        Prefix = variable.Prefix;
        IsHidden = variable.IsHidden;
        IsProtected = variable.IsProtected;
        NoPrefix = variable.NoPrefix;
    }

    [Description("The name of the variable")]
    public string Name { get; set; } = string.Empty;

    [Description("The help text for the variable")]
    public string? HelpText { get; set; } = string.Empty;

    [Description("Used prefix for the variable")]
    public string? Prefix { get; set; } = string.Empty;

    [Description("Is the variable hidden?")]
    public bool IsHidden { get; set; }

    [Description("Is the variable protected?")]
    public bool IsProtected { get; set; }

    [Description("Does the variable have a prefix?")]
    public bool NoPrefix { get; set; }
}
