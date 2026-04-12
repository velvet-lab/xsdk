namespace xSdk.Data;

public static class DatabaseExtensions
{

    public static TDatabaseOptions? GetDatabaseOptions<TDatabaseOptions>(this IDatabase database)
        where TDatabaseOptions : class
    {
        if (database is Database realDatabase)
        {
            if (realDatabase._databaseOptions is TDatabaseOptions options)
            {
                return options;
            }
        }

        return default;
    }
}
