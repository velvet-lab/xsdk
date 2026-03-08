using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Authentication;

namespace xSdk.Plugins.Authentication;

public static partial class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddApiKeyRepository<TRepository>(this AuthenticationBuilder builder)
        where TRepository : class, IApiKeyHandler
    {
        builder.Services.AddTransient<IApiKeyHandler, TRepository>();

        return builder;
    }
}
