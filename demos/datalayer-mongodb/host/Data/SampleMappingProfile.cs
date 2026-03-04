using xSdk.Data;
using xSdk.Data.Converters.Mapper;

namespace xSdk.Demos.Data
{
    internal sealed class SampleMappingProfile : MappingProfile
    {
        protected override void Configure()
        {
            CreateMap<SampleEntity, SampleModel>()
                .Map(dest => dest.AdditionalData, src => ExtensionDataConverter.Convert(src.ExtensionData));
                
            CreateMap<SampleModel, SampleEntity>()
                .Map(dest => dest.ExtensionData, src => ExtensionDataConverter.Convert(src.AdditionalData));

        }
    }
}
