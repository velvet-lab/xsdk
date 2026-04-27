using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.DataProtection;
using xSdk.Extensions.IO;
using xSdk.Extensions.Plugin;

namespace xSdk.Plugins.DataProtection;

internal class DefaultDataProtectionBuilder(IFileSystemService fileSystemService) : PluginBuilder, IDataProtectionPluginBuilder
{
    public void ConfigureDataProtection(IDataProtectionBuilder builder)
    {
        var keysLocation = GetKeyFolder();
        builder.PersistKeysToFileSystem(new DirectoryInfo(keysLocation));
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
            keyFolder = fileSystemService.Machine.Data.GetFullPath("/keys");
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

                keyFolder = fileSystemService.User.Data.GetFullPath("/keys");
                if (!Directory.Exists(keyFolder))
                    Directory.CreateDirectory(keyFolder);
            }
        }

        return keyFolder;
    }
}
