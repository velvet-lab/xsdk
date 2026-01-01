using xSdk.Extensions.Plugin;
using xSdk.Extensions.IO;
using xSdk.Hosting;
using xSdk.Shared;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace xSdk.Plugins.DataProtection
{
    public class DataProtectionPlugin : PluginBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            Logger.Info("Configure DataProtection");

            var setup = SlimHost.Instance.VariableSystem.GetSetup<DataProtectionSetup>();

            setup.Validate();

            IDataProtectionBuilder? builder = null;
            if (!string.IsNullOrEmpty(setup.ApplicationDiscriminator))
                builder = services.AddDataProtection(_ => _.ApplicationDiscriminator = setup.ApplicationDiscriminator);
            else
                builder = services.AddDataProtection();

            if (!string.IsNullOrEmpty(setup.ApplicationName))
                builder.SetApplicationName(setup.ApplicationName);

            if (!string.IsNullOrEmpty(setup.KeyLifetime))
            {
                if (TimeSpanParser.TryParse(setup.KeyLifetime, out TimeSpan result))
                {
                    builder.SetDefaultKeyLifetime(result);
                }
            }

            if (!SlimHost.Instance.PluginSystem.Invoke<IDataProtectionPluginBuilder>(x => x.ConfigureDataProtection(builder)))
            {
                var keysLocation = GetKeyFolder();
                builder.PersistKeysToFileSystem(new DirectoryInfo(keysLocation));
            }
        }

        private string GetKeyFolder()
        {
            Logger.Info("Try to get Key Folder for Data Protection");

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
                    Logger.Warn("KeyFolder '{0}' could not created. Create the Keyfolder in Users Home Profile.", keyFolder);

                    keyFolder = SlimHost.Instance.FileSystem.User.Data.GetFullPath("/keys");
                    if (!Directory.Exists(keyFolder))
                        Directory.CreateDirectory(keyFolder);
                }
            }

            return keyFolder;
        }
    }
}
