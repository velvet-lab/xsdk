using xSdk.Data.Converters.Mapper;

namespace xSdk.Data.Mocks
{
    internal class TestMappingProfile : MappingProfile
    {
        protected override void Configure()
        {
            CreateMap<TestEntity, TestModel>()
                .Ignore(dest => dest.PrimaryKey)
                .Map(dest => dest.Id, src => ObjectIdConverter.Convert(src.Id))
                .Map(dest => dest.MyName, src => src.Name)
                .Map(dest => dest.MyAge, src => src.Age);

            CreateMap<TestModel, TestEntity>()
                .Map(dest => dest.Name, src => src.MyName)
                .Map(dest => dest.Age, src => src.MyAge)
                .Map(dest => dest.Id, src => ObjectIdConverter.Convert(src.Id))
                .Ignore(dest => dest.PrimaryKey);
        }
    }
}
