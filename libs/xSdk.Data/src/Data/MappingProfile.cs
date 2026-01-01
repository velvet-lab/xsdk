using AutoMapper;
using NLog;

namespace xSdk.Data
{
    public static class MappingProfile
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static IMapper CreateMapper<TProfile>()
            where TProfile : Profile, new()
        {
            logger.Trace("Create Mapper for Profile '{0}'", typeof(TProfile));
            var config = new MapperConfiguration(cfg =>
            {
                cfg.Configure();
                cfg.AddProfile<TProfile>();
            });

            return config.CreateMapper();
        }

        public static IMapper CreateMapper(Action<Profile> configure)
        {
            logger.Trace("Create Mapper from Template with Post Configure Options");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.Configure();
                var template = new TemplateProfile();
                configure(template);
                cfg.AddProfile(template);
            });

            return config.CreateMapper();
        }

        internal static void Configure(this IMapperConfigurationExpression cfg)
        {
            // don't map any fields
            cfg.ShouldMapField = fi => false;

            // map properties with a public or private getter
            cfg.ShouldMapProperty = pi => pi.GetMethod != null && (pi.GetMethod.IsPublic);

            // don't map methods
            cfg.ShouldMapMethod = mi => false;

            cfg.AllowNullCollections = false;
        }
    }

    class TemplateProfile : Profile
    {
        public TemplateProfile() { }
    }
}
