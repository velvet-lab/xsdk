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

using System.Globalization;
using LiteDB;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Data;

[VariablePrefix("nosql")]
public sealed class NoSqlDatabaseOptions : VariableSetup
{
    protected override void OnInitialize()
    {
        InitialSize = 0;
        Upgrade = false;
        ReadOnly = false;
        Collation = new Collation(CultureInfo.CurrentCulture.LCID, CompareOptions.IgnoreCase);
    }
    
    [Variable(
        name: Definitions.FileName.Name,
        template: Definitions.FileName.Template,
        helpText: Definitions.FileName.HelpText
    )]
    public string? FileName        
    {
        get => ReadValue<string>(Definitions.FileName.Name);
        set => SetValue(Definitions.FileName.Name, value);
    }

    [Variable(
        name: Definitions.Password.Name,
        template: Definitions.Password.Template,
        helpText: Definitions.Password.HelpText
    )]
    public string? Password
    {
        get => ReadValue<string>(Definitions.Password.Name);
        set => SetValue(Definitions.Password.Name, value);
    }

    [Variable(
        name: Definitions.InitialSize.Name,
        template: Definitions.InitialSize.Template,
        helpText: Definitions.InitialSize.HelpText
    )]
    public long InitialSize
    {
        get => ReadValue<long>(Definitions.InitialSize.Name);
        set => SetValue(Definitions.InitialSize.Name, value);
    }

    [Variable(
        name: Definitions.Upgrade.Name,
        template: Definitions.Upgrade.Template,
        helpText: Definitions.Upgrade.HelpText
    )]
    public bool Upgrade
    {
        get => ReadValue<bool>(Definitions.Upgrade.Name);
        set => SetValue(Definitions.Upgrade.Name, value);
    }

    [Variable(
        name: Definitions.ReadOnly.Name,
        template: Definitions.ReadOnly.Template,
        helpText: Definitions.ReadOnly.HelpText
    )]
    public bool ReadOnly
    {
        get => ReadValue<bool>(Definitions.ReadOnly.Name);
        set => SetValue(Definitions.ReadOnly.Name, value);
    }

    public Collation Collation { get; set; }

    private static class Definitions
    {
        public static class FileName
        {
            public const string Name = "file-name";
            public const string Template = "--file-name <file-name>";
            public const string HelpText = "Specifies the file name for the NoSQL _database.";
        }

        public static class Password
        {
            public const string Name = "password";
            public const string Template = "--password <password>";
            public const string HelpText = "Specifies the password for the NoSQL _database.";
        }

        public static class InitialSize
        {
            public const string Name = "initial-size";
            public const string Template = "--initial-size <size>";
            public const string HelpText = "Specifies the initial size of the NoSQL _database in bytes.";
        }

        public static class Upgrade
        {
            public const string Name = "upgrade";
            public const string Template = "--upgrade <true|false>";
            public const string HelpText = "Indicates whether to upgrade the NoSQL _database if it already exists.";
        }

        public static class ReadOnly
        {
            public const string Name = "read-only";
            public const string Template = "--read-only <true|false>";
            public const string HelpText = "Indicates whether to open the NoSQL _database in read-only mode.";
        }


    }
}
