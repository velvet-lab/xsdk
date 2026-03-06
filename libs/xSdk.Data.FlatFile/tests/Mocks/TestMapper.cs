//using xSdk.Data.Converters.Mapper;

//namespace xSdk.Data.Mocks
//{
//    internal class TestMapper : Profile
//    {
//        public TestMapper()
//        {
//            this.CreateMap<TestEntity, TestModel>()
//                .ForMember(x => x.PrimaryKey, opts => opts.Ignore())
//                .ForMember(x => x.Id, opts => opts.ConvertUsing(new GuidConverter.ToEntityProperty()))
//                .ForMember(x => x.MyName, opts => opts.MapFrom(x => x.Name))
//                .ForMember(x => x.MyAge, opts => opts.MapFrom(x => x.Age))
//                .ReverseMap()
//                .ForMember(x => x.Name, opts => opts.MapFrom(x => x.MyName))
//                .ForMember(x => x.Age, opts => opts.MapFrom(x => x.MyAge))
//                .ForMember(x => x.Id, opts => opts.ConvertUsing(new GuidConverter.ToModelProperty()))
//                .ForMember(x => x.PrimaryKey, opts => opts.Ignore());
//        }
//    }
//}
