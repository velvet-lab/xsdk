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

[VariablePrefix("vault-username-password-auth")]
public sealed class UsernamePasswordAuth : VariableSetup
{
    [Variable(
        name: Definitions.Username.Name,
        template: Definitions.Username.Template,
        helpText: Definitions.Username.HelpText,
        hidden: true)]
    public string? Username
    {
        get => ReadValue<string>(Definitions.Username.Name);
        set => SetValue(Definitions.Username.Name, value);
    }

    [Variable(
        name: Definitions.Password.Name,
        template: Definitions.Password.Template,
        helpText: Definitions.Password.HelpText,
        hidden: true)]
    public string? Password
    {
        get => ReadValue<string>(Definitions.Password.Name);
        set => SetValue(Definitions.Password.Name, value);
    }

    private static class Definitions
    {
        public static class Username
        {
            public const string Name = nameof(Username);
            public const string Template = $"--vault-username <username>";
            public const string HelpText = "Username for username/password auth to access vault";
        }

        public static class Password
        {
            public const string Name = nameof(Password);
            public const string Template = $"--vault-password <password>";
            public const string HelpText = "Password for username/password auth to access vault";
        }
    }
}
