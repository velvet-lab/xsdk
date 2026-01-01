using AutoMapper;

namespace xSdk.Data
{
    internal class ModelMappingProfile<TModel> : Profile
        where TModel : IModel
    {
        // Tricky: This Profile exists only to copy Entity to Entity without the ID

        public ModelMappingProfile()
        {
            this.CreateMap<TModel, TModel>().ForMember(x => x.Id, opts => opts.Ignore()).ForMember(x => x.PrimaryKey, opts => opts.Ignore());
        }
    }
}
