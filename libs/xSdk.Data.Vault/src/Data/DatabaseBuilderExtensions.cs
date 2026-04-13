using System.Xml.Linq;
using xSdk.Extensions.Variable;

namespace xSdk.Data;

public static class DatabaseBuilderExtensions
{
    public static IDatabaseBuilder ConfigureAuth<TAuthOptions>(this IDatabaseBuilder builder, Action<TAuthOptions>? configure)
        where TAuthOptions : class, IVariableSetup
        => builder.ConfigureOptions<TAuthOptions>(null, options =>
        {
            configure?.Invoke(options);
        });

    public static IDatabaseBuilder ConfigureAuth<TAuthOptions>(this IDatabaseBuilder builder, string? name, Action<TAuthOptions>? configure)
        where TAuthOptions : class, IVariableSetup
        => builder.ConfigureOptions<TAuthOptions>(name, options =>
        {
            configure?.Invoke(options);
        });
}
