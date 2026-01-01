using AutoMapper;

namespace xSdk.Data
{
    internal class EntityMappingProfile<TEntity> : Profile
        where TEntity : IEntity
    {
        // Tricky: This Profile exists only to copy Entity to Entity without the ID

        public EntityMappingProfile()
        {
            CreateMap<TEntity, TEntity>().ForMember(x => x.Id, opts => opts.Ignore()).ForMember(x => x.PrimaryKey, opts => opts.Ignore());
        }
    }
}
