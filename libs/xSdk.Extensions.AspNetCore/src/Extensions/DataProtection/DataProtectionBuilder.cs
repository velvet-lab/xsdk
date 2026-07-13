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
using xSdk.Extensions.Builder;
using xSdk.Extensions.IO;
using xSdk.Extensions.Logging;

namespace xSdk.Extensions.DataProtection;

public sealed class DataProtectionBuilder : BuilderBase
{
    private static ILogger Logger => field ??= LogManager.CreateLogger<DataProtectionBuilder>();

    internal Action<IDataProtectionBuilder>? ConfigureDataProtectionAction { get => field ?? (_ => ConfigureDataProtection(_)); set; }

    private void ConfigureDataProtection(IDataProtectionBuilder builder)
    {
        string keysLocation = GetKeyFolder();
        builder.PersistKeysToFileSystem(new DirectoryInfo(keysLocation));
    }

    private string GetKeyFolder()
    {
        Logger.LogInformation("Try to get Key Folder for Data Protection");

        IFileSystemService fileSystemService = Services.GetRequiredService<IFileSystemService>();

        string? keyFolder;
        if (Debugger.IsAttached)
        {
            keyFolder = Path.Combine(FileSystemHelper.GetExecutingFolder(), "keys");
        }
        else
        {
            keyFolder = fileSystemService.Machine.Data.GetFullPath("/keys");
        }

        if (!string.IsNullOrEmpty(keyFolder) && !Directory.Exists(keyFolder))
        {
            try
            {
                Directory.CreateDirectory(keyFolder);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "KeyFolder '{keyFolder}' could not created. Create the Keyfolder in Users Home Profile.", keyFolder);

                keyFolder = fileSystemService.User.Data.GetFullPath("/keys");
                if (!Directory.Exists(keyFolder))
                {
                    Directory.CreateDirectory(keyFolder);
                }
            }
        }

        return keyFolder;
    }
}
