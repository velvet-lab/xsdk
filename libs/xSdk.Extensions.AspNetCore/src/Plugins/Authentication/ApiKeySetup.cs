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

namespace xSdk.Plugins.Authentication;

[VariablePrefix("auth_apikey")]
public class ApiKeySetup : Setup
{
    [Variable(
        name: Definitions.Realm.Name,
        template: Definitions.Realm.Template,
        helpText: Definitions.Realm.HelpText,
        defaultValue: Definitions.Realm.DefaultValue
    )]
    public string Realm
    {
        get => ReadValue<string>(Definitions.Realm.Name);
        set => SetValue(Definitions.Realm.Name, value);
    }

    protected override void ValidateSetup()
    {
        this.ValidateMember(x => string.IsNullOrEmpty(x.Realm), "Authentication realm is missing", Definitions.Realm.Name);
    }

    public static class Definitions
    {
        public static class Realm
        {
            public const string Name = "realm";
            public const string Template = "--realm <realm>";
            public const string HelpText = "A realm for Authenication";
            public const string DefaultValue = "xSdk Authentication Realm";
        }
    }
}
