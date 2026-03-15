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

namespace xSdk.Demos;

[VariableNoPrefix()]
public sealed class SetupWithoutPrefix : Setup
{
    [Variable(
        name: Definitions.Prop1.Name,
        template: Definitions.Prop1.Template,
        protect: true,
        noPrefix: true)]
    public string NoAppPrefix_NoSetupPrefix
    {
        get => ReadValue<string>(Definitions.Prop1.Name);
        set => SetValue(Definitions.Prop1.Name, value);
    }

    [Variable(
        name: Definitions.Prop2.Name,
        template: Definitions.Prop2.Template,
        protect: true)]
    public string WithAppPrefix_NoSetupPrefix
    {
        get => ReadValue<string>(Definitions.Prop2.Name);
        set => SetValue(Definitions.Prop2.Name, value);
    }

    internal static class Definitions
    {
        public static class Prop1
        {
            public const string Name = "no-app-prefix-no-setup-prefix";
            public const string Template = $"--no-app-prefix-no-setup-prefix <services>";
        }

        public static class Prop2
        {
            public const string Name = "with-app-prefix-no-setup-prefix";
            public const string Template = $"--with-app-prefix-no-setup-prefix <services>";
        }
    }
}
