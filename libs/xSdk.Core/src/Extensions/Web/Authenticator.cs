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

using System.Diagnostics.CodeAnalysis;
using RestSharp.Authenticators;
using xSdk.Security;

namespace xSdk.Extensions.Web;

[ExcludeFromCodeCoverage]
public static class Authenticator
{
    public static HttpBasicAuthenticator GetAuthenticator<TCredentials>()
        where TCredentials : Credentials, new()
    {
        //var appPrefix = SlimHost.Instance.AppPrefix;

        //return GetAuthenticator<TCredentials>(appPrefix);
        throw new NotImplementedException();
    }

    public static HttpBasicAuthenticator GetAuthenticator<TCredentials>(string securityContext)
        where TCredentials : Credentials, new()
    {
        var creds = CredentialManager.LoadCredentials<TCredentials>(securityContext);
        if (creds == null)
        {
            throw new SdkException("No credentials found to access artifactory");
        }

        var user = creds.User;
        var token = creds.Token;

        if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(token))
        {
            return new HttpBasicAuthenticator(user, token);
        }

        if (string.IsNullOrEmpty(user))
            throw new SdkException("No Artifactory User given");

        if (string.IsNullOrEmpty(token))
            throw new SdkException("No Artifactory token given");

        throw new SdkException("Authenticator could not created");
    }
}
