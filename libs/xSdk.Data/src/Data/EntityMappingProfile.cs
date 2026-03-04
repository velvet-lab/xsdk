namespace xSdk.Data
{
    internal class EntityMappingProfile<TEntity> : MappingProfile
        where TEntity : IEntity
    {
        // Tricky: This Profile exists only to copy Entity to Entity without the ID

        protected override void Configure()
        {
            CreateMap<TEntity, TEntity>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.PrimaryKey);
        }
    }
}
