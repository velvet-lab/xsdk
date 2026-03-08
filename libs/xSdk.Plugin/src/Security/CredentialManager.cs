using NLog;
using xSdk.Hosting;

namespace xSdk.Security;

public static class CredentialManager
{
    private static readonly object LockObject = new object();
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private static string Context => GetContext();

    public static TCredentials LoadCredentialsAsync<TCredentials>()
        where TCredentials : Credentials, new() => LoadCredentials<TCredentials>(Context);

    public static TCredentials LoadCredentials<TCredentials>(string context)
        where TCredentials : Credentials, new()
    {
        _logger.Info("Try to load encrypted credentials");

        var credsFile = GetCredsFileName(context);
        if (File.Exists(credsFile))
        {
            return CryptoTool.Decrypt<TCredentials>(credsFile, context);
        }
        else
        {
            _logger.Warn("No saved credentials found");
        }
        return new TCredentials();
    }

    public static void SaveCredentials<TCredentials>(TCredentials credentials)
        where TCredentials : Credentials => SaveCredentials<TCredentials>(credentials, Context);

    public static void SaveCredentials<TCredentials>(TCredentials credentials, string context)
        where TCredentials : Credentials
    {
        try
        {
            var credsFile = GetCredsFileName(context);
            CryptoTool.Encrypt<TCredentials>(credsFile, credentials, context);
        }
        catch (Exception ex)
        {
            _logger.Warn("Credentials could not saved. (Reason: {0})", ex.Message);
        }
    }

    public static void Reset() => Reset(Context);

    public static void Reset(string context)
    {
        _logger.Info("Reset encrypted credentials");
        var credsFile = GetCredsFileName(context);
        if (File.Exists(credsFile))
        {
            File.Delete(credsFile);
        }
    }

    private static string GetCredsFileName(string context)
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(userProfile, $".{context}rc");
    }

    private static string GetContext() => SlimHost.Instance.AppPrefix;
}
