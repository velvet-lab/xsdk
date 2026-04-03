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

using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.DataProtection;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Plugins.DataProtection;

internal sealed class DataProtectionPluginHost : PluginHost<DataProtectionSetup>
{
    public override void ConfigureServices(IServiceCollection services)
    {
        Logger.LogInformation("Configure DataProtection");

        IDataProtectionBuilder? builder = null;
        if (!string.IsNullOrEmpty(Setup.ApplicationDiscriminator))
            builder = services.AddDataProtection(_ => _.ApplicationDiscriminator = Setup.ApplicationDiscriminator);
        else
            builder = services.AddDataProtection();

        if (!string.IsNullOrEmpty(Setup.ApplicationName))
            builder.SetApplicationName(Setup.ApplicationName);

        if (!string.IsNullOrEmpty(Setup.KeyLifetime))
        {
            if (TimeSpanParser.TryParse(Setup.KeyLifetime, out TimeSpan result))
            {
                builder.SetDefaultKeyLifetime(result);
            }
        }

        if (!SlimHost.Instance.PluginSystem.ConfigurePlugin<IDataProtectionPluginBuilder>(x => x.ConfigureDataProtection(builder)))
        {
            var keysLocation = GetKeyFolder();
            builder.PersistKeysToFileSystem(new DirectoryInfo(keysLocation));
        }
    }

    private string GetKeyFolder()
    {
        Logger.LogInformation("Try to get Key Folder for Data Protection");

        string? keyFolder = null;
        if (Debugger.IsAttached)
        {
            keyFolder = Path.Combine(FileSystemHelper.GetExecutingFolder(), "keys");
        }
        else
        {
            keyFolder = SlimHost.Instance.FileSystem.Machine.Data.GetFullPath("/keys");
        }

        if (!string.IsNullOrEmpty(keyFolder) && !Directory.Exists(keyFolder))
        {
            try
            {
                Directory.CreateDirectory(keyFolder);
            }
            catch
            {
                Logger.LogWarning("KeyFolder '{0}' could not created. Create the Keyfolder in Users Home Profile.", keyFolder);

                keyFolder = SlimHost.Instance.FileSystem.User.Data.GetFullPath("/keys");
                if (!Directory.Exists(keyFolder))
                    Directory.CreateDirectory(keyFolder);
            }
        }

        return keyFolder;
    }
}
