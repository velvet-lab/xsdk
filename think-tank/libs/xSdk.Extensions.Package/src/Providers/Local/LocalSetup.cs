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

using xSdk.Extensions.IO;
using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Hosting;

namespace xSdk.Extensions.Package.Providers.Local
{
    public class LocalSetup : Setup
    {
        [Variable(name: Definitions.Path.Name, template: Definitions.Path.Template, helpText: Definitions.Path.HelpText, prefix: Definitions.Path.Prefix)]
        public string Path
        {
            get => this.ReadValue<string>(Definitions.Path.Name);
            set => this.SetValue(Definitions.Path.Name, value);
        }

        protected override void Initialize()
        {
            if (string.IsNullOrEmpty(Path))
            {
                Path = SlimHost.Instance.FileSystem.User.Data.GetFullPath("/cache");
            }
        }

        public static class Definitions
        {
            public static class Path
            {
                public const string Name = "path";
                public const string Template = "--path <cache>";
                public const string HelpText = "Location for the package";
                public const string Prefix = "local";
            }
        }
    }
}
