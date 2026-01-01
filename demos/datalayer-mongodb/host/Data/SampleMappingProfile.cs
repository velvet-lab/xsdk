using AutoMapper;
using xSdk.Data.Converters.Mapper;

namespace xSdk.Demos.Data
{
    internal sealed class SampleMappingProfile : Profile
    {
        public SampleMappingProfile()
        {
            CreateMap<SampleEntity, SampleModel>()
                .ForMember(x => x.AdditionalData, opts => opts.ConvertUsing(new ExtensionDataConverter.ToModelProperty(), x => x.ExtensionData))
                .ReverseMap()
                .ForMember(x => x.ExtensionData, opts => opts.ConvertUsing(new ExtensionDataConverter.ToEntityProperty(), x => x.AdditionalData));

        }
    }
}
