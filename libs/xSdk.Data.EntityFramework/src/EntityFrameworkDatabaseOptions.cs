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

using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Data;

[VariablePrefix("ef")]

public class EntityFrameworkDatabaseOptions : DatabaseOptions
{
    protected override void OnInitialize()
    {
        TransactionsEnabled = true;
    }

    [Variable(
        name: Definitions.TransactionsEnabled.Name,
        template: Definitions.TransactionsEnabled.Template,
        helpText: Definitions.TransactionsEnabled.HelpText,
        defaultValue: Definitions.TransactionsEnabled.DefaultValue
    )]
    public bool TransactionsEnabled
    {
        get => ReadValue<bool>(Definitions.TransactionsEnabled.Name);
        set => SetValue(Definitions.TransactionsEnabled.Name, value);
    }

    private static partial class Definitions
    {
        public static class TransactionsEnabled
        {
            public const string Name = "transactions-enabled";
            public const string Template = "--transactions-enabled <true|false>";
            public const string HelpText = "Indicates whether transactions are enabled for the Entity Framework database.";
            public const bool DefaultValue = true;
        }
    }
}
