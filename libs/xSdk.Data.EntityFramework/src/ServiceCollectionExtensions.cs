using Microsoft.EntityFrameworkCore;

namespace xSdk.Data;

public static class ServiceCollectionExtensions
{
    public static IDatalayerBuilder UseEntityFramework<TDbContext>(this IDatalayerBuilder builder, string name)
        where TDbContext : DbContext =>
        builder.UseDatabase<EntityFrameworkDatabase<TDbContext>, EntityFrameworkDatabaseSetup, EntityFrameworkConnectionBuilder>(name);

    public static IDatalayerBuilder UseEntityFramework<TDbContext>(
        this IDatalayerBuilder builder,
        string name,
        Action<EntityFrameworkDatabaseSetup> configure
    )
        where TDbContext : DbContext =>
        builder.UseDatabase<EntityFrameworkDatabase<TDbContext>, EntityFrameworkDatabaseSetup, EntityFrameworkConnectionBuilder>(name, configure);
}
