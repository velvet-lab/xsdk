using Mapster;
using MapsterMapper;
using NLog;

namespace xSdk.Data
{
    public static class MappingFactory
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static IMapper CreateMapper<TProfile>()
            where TProfile : MappingProfile, new()
        {
            logger.Trace("Create Mapper for Profile '{0}'", typeof(TProfile));

            var profile = new TProfile();
            var config = profile.CreateConfig();
            var mapper = new Mapper(config);

            return mapper;
        }

        public static IMapper CreateMapper<TProfile>(Action<TypeAdapterConfig> configure)
            where TProfile : MappingProfile, new()
        {
            logger.Trace("Create Mapper for Profile '{0}' with custom configuration", typeof(TProfile));

            var profile = new TProfile();
            var config = profile.CreateConfig(configure);
            var mapper = new Mapper(config);

            return mapper;
        }
    }
}
