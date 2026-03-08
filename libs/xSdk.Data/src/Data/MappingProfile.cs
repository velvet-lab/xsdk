using Mapster;
using NLog;

namespace xSdk.Data;

public abstract class MappingProfile
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    protected static TypeAdapterSetter<TSource, TDestination> CreateMap<TSource, TDestination>()
    {
        TypeAdapterConfig<TSource, TDestination>.Clear();
        return TypeAdapterConfig<TSource, TDestination>.NewConfig();
    }

    internal TypeAdapterConfig CreateConfig(Action<TypeAdapterConfig>? configure = null)
    {
        // default configuration
        Action<TypeAdapterConfig> defaultConfig = config =>
        {
            config.Default.EnableNonPublicMembers(false);
            config.Default.IgnoreNullValues(true);
            config.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
            config.Default.PreserveReference(true);
            config.Default.Settings.EnableNonPublicMembers = false;

            config.RequireExplicitMapping = true;
            config.RequireDestinationMemberSource = false;
        };

        logger.Debug("Creating TypeAdapterConfig for Profile '{0}'", GetType());
        var globalConfig = TypeAdapterConfig.GlobalSettings;

        if (configure == null)
        {
            logger.Debug("Using default configuration for Profile '{0}'", GetType());
            configure = defaultConfig;
        }

        logger.Debug("Applying global configuration for Profile '{0}'", GetType());
        configure(globalConfig);

        logger.Debug("Applying profile configuration for Profile '{0}'", GetType());
        Configure(globalConfig);

        logger.Debug("Compiling TypeAdapterConfig for Profile '{0}'", GetType());
        globalConfig.Compile();

        return globalConfig;
    }

    protected virtual void Configure()
    {

    }

    protected virtual void Configure(TypeAdapterConfig config)
    {
        Configure();
    }
}
