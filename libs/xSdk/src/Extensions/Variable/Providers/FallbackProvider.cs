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

using System.Runtime.InteropServices;
using xSdk.Extensions.Options;
using xSdk.Hosting;

namespace xSdk.Extensions.Variable.Providers;

internal sealed class FallbackProvider(ApplicationOptions options) : VariableProviderBase
{
    protected override bool ExistsVariable(IVariable variable)
    {
        var envFile = CreateFileName(variable);
        return File.Exists(envFile);
    }

    protected override object ReadVariable(IVariable variable)
    {
        string result = default;

        var envFile = CreateFileName(variable);
        if (File.Exists(envFile))
        {
            result = File.ReadAllText(envFile);
        }

        return result;
    }

    private string CreateFileName(IVariable variable) =>
        Path.Combine(GetDefaultProfilePath(), options.Prefix.ToLower(), "config", $"{variable.Name}.env");

    private static string GetDefaultProfilePath()
    {
        // Create also a Folder Structure in Default Profile Folder, but only when Runtime is User Runtime
        var defaultProfileBase = "/etc/skel/.local/share";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var userProfileBase = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            defaultProfileBase = Path.Combine(userProfileBase.FullName, "Default", "AppData", "Local");
        }

        return defaultProfileBase;
    }
}
