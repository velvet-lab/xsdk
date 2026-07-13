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
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Commands;

[VariablePrefix("console")]
public class ConsoleOptions : OptionsBase
{
    [Variable(
            name: Definitions.DisableDefaultHelp.Name,
            template: Definitions.DisableDefaultHelp.Template,
            helpText: Definitions.DisableDefaultHelp.HelpText
        )]
    public bool DisableDefaultHelp
    {
        get => ReadValue<bool>(Definitions.DisableDefaultHelp.Name);
        set => SetValue(Definitions.DisableDefaultHelp.Name, value);
    }

    public static class Definitions
    {
        public static class DisableDefaultHelp
        {
            public const string Name = nameof(DisableDefaultHelp);
            public const string Template = "--disable-help";
            public const string HelpText = "Whether to disable the default help";
        }
    }
}
