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

using xSdk.Extensions.Plugin;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.WebSecurity;

public sealed class WebSecurityPluginOptions : PluginOptions
{
    [Variable(
        name: Definitions.Origins.Name,
        template: Definitions.Origins.Template,
        helpText: Definitions.Origins.HelpText,
        protect: true)]
    public string? Origins
    {
        get => ReadValue<string>(Definitions.Origins.Name);
        set => SetValue(Definitions.Origins.Name, value);
    }

    public bool IsCorsEnabled => !string.IsNullOrEmpty(Origins);

    public static class Definitions
    {
        public static class Origins
        {
            public const string Name = "origins";
            public const string Template = "--origins <origins>";
            public const string HelpText = "Comma seperated list of origins";
        }
    }
}
