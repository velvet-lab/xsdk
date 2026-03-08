using Mapster;

namespace xSdk.Data;

internal class ModelMappingProfile<TModel> : MappingProfile
    where TModel : IModel
{
    // Tricky: This Profile exists only to copy Entity to Entity without the ID

    protected override void Configure()
    {
        CreateMap<TModel, TModel>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PrimaryKey);
    }
}
