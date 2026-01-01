using AspNetCore.Authentication.ApiKey;

namespace xSdk.Plugins.Authentication
{
    public static class AuthenticationDefaults
    {
        internal const string DefaultScheme = "NotConfigured";

        public static class ApiKeyAuth
        {
            public const string Name = "API Key Authentication";

            public static class InHeader
            {
                public const string Header = "X-API-KEY";
                public const string Scheme = $"{ApiKeyDefaults.AuthenticationScheme}InHeaderScheme";
            }

            public static class InAuthorizationHeader
            {
                public const string Header = "ApiKeyName";
                public const string Scheme = $"{ApiKeyDefaults.AuthenticationScheme}InAuthorizationHeaderScheme";
            }
        }

        public static class MulitAuth
        {
            public const string Scheme = "MultiAuthScheme";
        }
    }
}
