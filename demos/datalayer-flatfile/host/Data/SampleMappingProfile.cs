using xSdk.Data;

namespace xSdk.Demos.Data;

internal sealed class SampleMappingProfile : MappingProfile
{
    protected override void Configure()
    {
        CreateMap<SampleEntity, SampleModel>();
        CreateMap<SampleModel, SampleEntity>();
    }
}


