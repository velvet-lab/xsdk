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

[VariablePrefix("flat-file")]
public class FlatFileDatabaseOptions : VariableSetup
{
    protected override void OnInitialize()
    {
        UseLowerCamelCase = true;
        ReloadBeforeGetCollection = false;
    }

    [Variable(
        name: Definitions.FilePath.Name,
        template: Definitions.FilePath.Template,
        helpText: Definitions.FilePath.HelpText        
    )]
    public string FilePath
    {
        get => ReadValue<string>(Definitions.FilePath.Name) ?? string.Empty;
        set => SetValue(Definitions.FilePath.Name, value);
    }

    [Variable(
        name: Definitions.UseLowerCamelCase.Name,
        template: Definitions.UseLowerCamelCase.Template,
        helpText: Definitions.UseLowerCamelCase.HelpText,
        defaultValue: Definitions.UseLowerCamelCase.DefaultValue
    )]
    public bool UseLowerCamelCase
    {
        get => ReadValue<bool>(Definitions.UseLowerCamelCase.Name);
        set => SetValue(Definitions.UseLowerCamelCase.Name, value);
    }

    [Variable(
        name: Definitions.ReloadBeforeGetCollection.Name,
        template: Definitions.ReloadBeforeGetCollection.Template,
        helpText: Definitions.ReloadBeforeGetCollection.HelpText
    )]
    public bool ReloadBeforeGetCollection
    {
        get => ReadValue<bool>(Definitions.ReloadBeforeGetCollection.Name);
        set => SetValue(Definitions.ReloadBeforeGetCollection.Name, value);
    }

    [Variable(
        name: Definitions.KeyProperty.Name,
        template: Definitions.KeyProperty.Template,
        helpText: Definitions.KeyProperty.HelpText
    )]
    public string? KeyProperty
    {
        get => ReadValue<string>(Definitions.KeyProperty.Name);
        set => SetValue(Definitions.KeyProperty.Name, value);
    }

    [Variable(
        name: Definitions.EncryptionKey.Name,
        template: Definitions.EncryptionKey.Template,
        helpText: Definitions.EncryptionKey.HelpText
    )]
    public string? EncryptionKey
    {
        get => ReadValue<string>(Definitions.EncryptionKey.Name);
        set => SetValue(Definitions.EncryptionKey.Name, value);
    }

    [Variable(
        name: Definitions.MinifyJson.Name,
        template: Definitions.MinifyJson.Template,
        helpText: Definitions.MinifyJson.HelpText
    )]
    public bool MinifyJson
    {
        get => ReadValue<bool>(Definitions.MinifyJson.Name);
        set => SetValue(Definitions.MinifyJson.Name, value);
    }

    private static partial class Definitions
    {
        public static class FilePath
        {
            public const string Name = "file-path";
            public const string Template = "--path <path>";
            public const string HelpText = "Path to the folder where the flat files are stored.";
        }

        public static class UseLowerCamelCase
        {
            public const string Name = "use-lower-camel-case";
            public const string Template = "--use-lower-camel-case";
            public const string HelpText = "Whether to use lower camel case for property names in the flat file.";
            public const bool DefaultValue = true;
        }

        public static class ReloadBeforeGetCollection
        {
            public const string Name = "reload-before-get-collection";
            public const string Template = "--reload-before-get-collection";
            public const string HelpText = "Whether to reload the flat file before getting a collection.";
        }

        public static class KeyProperty
        {
            public const string Name = "key-property";
            public const string Template = "--key-property <property-name>";
            public const string HelpText = "The name of the property that is used as key in the flat file.";
        }

        public static class EncryptionKey
        {
            public const string Name = "encryption-key";
            public const string Template = "--encryption-key <key>";
            public const string HelpText = "The key used to encrypt the flat file. If not set, the flat file will not be encrypted.";
        }

        public static class MinifyJson
        {
            public const string Name = "minify-json";
            public const string Template = "--minify-json";
            public const string HelpText = "Whether to minify the JSON output in the flat file.";
        }
    }
}
