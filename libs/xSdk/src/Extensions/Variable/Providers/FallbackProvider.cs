using System.Runtime.InteropServices;
using xSdk.Hosting;

namespace xSdk.Extensions.Variable.Providers;

internal sealed class FallbackProvider() : VariableProviderBase
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
        Path.Combine(GetDefaultProfilePath(), SlimHost.Instance.AppPrefix.ToLower(), "config", $"{variable.Name}.env");

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
