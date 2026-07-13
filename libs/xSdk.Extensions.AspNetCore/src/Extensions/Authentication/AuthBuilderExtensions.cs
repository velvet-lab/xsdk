using Microsoft.AspNetCore.Authentication;

namespace xSdk.Extensions.Authentication;

public static class AuthBuilderExtensions
{
    extension(AuthBuilder builder)
    {
        public AuthBuilder WithAuthentication(Action<AuthenticationBuilder> configure)
        {
            builder.ConfigureAuthenticationAction = configure;
            return builder;
        }

        public AuthBuilder WithAuthorization(Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions> configure)
        {
            builder.ConfigureAuthorizationAction = configure;
            return builder;
        }

        public AuthBuilder WithSchemeSelector(Func<Microsoft.AspNetCore.Http.HttpContext, string?> selector)
        {
            builder.SelectAuthenticationSchemeAction = selector;
            return builder;
        }
    }
}
