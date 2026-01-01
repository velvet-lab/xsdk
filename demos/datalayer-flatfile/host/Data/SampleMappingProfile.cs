using AutoMapper;

namespace xSdk.Demos.Data
{
    internal sealed class SampleMappingProfile : Profile
    {
        public SampleMappingProfile()
        {
            CreateMap<SampleEntity, SampleModel>().ReverseMap();
        }
    }
}
