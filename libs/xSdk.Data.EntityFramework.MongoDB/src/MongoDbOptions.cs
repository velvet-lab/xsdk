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

using System.Text.Json.Serialization;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Data;

[VariablePrefix("mongodb")]
public class MongoDbOptions : DatabaseOptions
{
    [Variable(
        name: Definitions.Database.Name,
        template: Definitions.Database.Template,
        helpText: Definitions.Database.HelpText,
        hidden: true
        )]
    [JsonPropertyName("default_database")]
    public string? Database
    {
        get => ReadValue<string>(Definitions.Database.Name);
        set => SetValue(Definitions.Database.Name, value);
    }

    [Variable(
        name: Definitions.Username.Name,
        template: Definitions.Username.Template,
        helpText: Definitions.Username.HelpText,
        hidden: true
        )]
    [JsonPropertyName(Definitions.Username.Name)]
    public string? Username
    {
        get => ReadValue<string>(Definitions.Username.Name);
        set => SetValue(Definitions.Username.Name, value);
    }

    [Variable(
        name: Definitions.Password.Name,
        template: Definitions.Password.Template,
        helpText: Definitions.Password.HelpText,
        hidden: true
        )]
    [JsonPropertyName(Definitions.Password.Name)]
    public string? Password
    {
        get => ReadValue<string>(Definitions.Password.Name);
        set => SetValue(Definitions.Password.Name, value);
    }

    [Variable(
        name: Definitions.Hosts.Name,
        template: Definitions.Hosts.Template,
        helpText: Definitions.Hosts.HelpText,
        hidden: true
        )]
    [JsonPropertyName(Definitions.Hosts.Name)]
    public string[]? Hosts
    {
        get => ReadValue<string[]>(Definitions.Hosts.Name);
        set => SetValue(Definitions.Hosts.Name, value);
    }

    [Variable(
        name: Definitions.RootCertificate.Name,
        template: Definitions.RootCertificate.Template,
        helpText: Definitions.RootCertificate.HelpText,
        hidden: true
        )]
    [JsonPropertyName(Definitions.RootCertificate.Name)]
    public string? RootCertificate
    {
        get => ReadValue<string>(Definitions.RootCertificate.Name);
        set => SetValue(Definitions.RootCertificate.Name, value);
    }

    [Variable(
        name: Definitions.Uri.Name,
        template: Definitions.Uri.Template,
        helpText: Definitions.Uri.HelpText,
        hidden: true
        )]
    [JsonPropertyName(Definitions.Uri.Name)]
    public string? Uri
    {
        get => ReadValue<string>(Definitions.Uri.Name);
        set => SetValue(Definitions.Uri.Name, value);
    }

    internal static class Definitions
    {
        public static class Database
        {
            public const string Name = nameof(Database);
            public const string Template = $"--database <database>";
            public const string HelpText = "Connection String or CloudFoundry Binding Name for the Database";
        }

        public static class Username
        {
            public const string Name = nameof(Username);
            public const string Template = $"--username <username>";
            public const string HelpText = "Username for MongoDB Database";
        }

        public static class Password
        {
            public const string Name = nameof(Password);
            public const string Template = $"--password <password>";
            public const string HelpText = "Password for MongoDB Database";
        }

        public static class Hosts
        {
            public const string Name = nameof(Hosts);
            public const string Template = $"--hosts <host1,host2,host3>";
            public const string HelpText = "Hosts for MongoDB Database Cluster";
        }

        public static class RootCertificate
        {
            public const string Name = nameof(RootCertificate);
            public const string Template = $"--cacrt <value>";
            public const string HelpText = "Root RoleId which should client certificates trusted";
        }

        public static class Uri
        {
            public const string Name = nameof(Uri);
            public const string Template = $"--uri <uri>";
            public const string HelpText = "MongoDB Connection URI";
        }
    }
}
