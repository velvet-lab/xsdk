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

using Microsoft.Extensions.Logging;
using xSdk;
using xSdk.Hosting;

namespace xSdk.Security;

public static class CredentialManager
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private static string Context => throw new NotImplementedException(); // SlimHost.Instance.AppPrefix;

    public static TCredentials LoadCredentialsAsync<TCredentials>()
        where TCredentials : Credentials, new() => LoadCredentials<TCredentials>(Context);

    public static TCredentials LoadCredentials<TCredentials>(string context)
        where TCredentials : Credentials, new()
    {
        _logger.LogInformation("Try to load encrypted credentials");

        var credsFile = GetCredsFileName(context);
        if (File.Exists(credsFile))
        {
            return CryptoTool.Decrypt<TCredentials>(credsFile, context);
        }
        else
        {
            _logger.LogWarning("No saved credentials found");
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
            _logger.LogWarning("Credentials could not saved. (Reason: {message})", ex.Message);
        }
    }

    public static void Reset() => Reset(Context);

    public static void Reset(string context)
    {
        _logger.LogInformation("Reset encrypted credentials");
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
}
