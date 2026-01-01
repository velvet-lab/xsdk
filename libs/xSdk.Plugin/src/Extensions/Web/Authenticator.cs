using xSdk.Hosting;
using xSdk.Security;
using RestSharp.Authenticators;

namespace xSdk.Extensions.Web
{
    public static class Authenticator
    {
        public static HttpBasicAuthenticator GetAuthenticator<TCredentials>()
            where TCredentials : Credentials, new()
        {
            var appPrefix = SlimHost.Instance.AppPrefix;

            return GetAuthenticator<TCredentials>(appPrefix);
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
}
