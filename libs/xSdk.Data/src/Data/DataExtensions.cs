using AutoMapper;
using System.Text.Json;

namespace xSdk.Data
{
    public static class DataExtensions
    {
        #region From Model to Entity

        public static TEntity ToEntity<TProfile, TEntity>(this IModel model, Action<IMappingOperationOptions<IModel, TEntity>> opts = null)
            where TProfile : Profile, new()
            where TEntity : IEntity
        {
            var mapper = MappingProfile.CreateMapper<TProfile>();
            if (opts == null)
                return mapper.Map<IModel, TEntity>(model);
            else
                return mapper.Map(model, opts);
        }

        public static IEnumerable<TEntity> ToEntity<TProfile, TEntity>(
            this IEnumerable<IModel> models,
            Action<IMappingOperationOptions<IEnumerable<IModel>, IEnumerable<TEntity>>> opts = null
        )
            where TProfile : Profile, new()
            where TEntity : IEntity
        {
            var mapper = MappingProfile.CreateMapper<TProfile>();
            if (opts == null)
                return mapper.Map<IEnumerable<IModel>, IEnumerable<TEntity>>(models);
            else
                return mapper.Map(models, opts);
        }

        #endregion

        #region From Entity to Model

        public static TModel ToModel<TProfile, TModel>(this IEntity entity, Action<IMappingOperationOptions<IEntity, TModel>> opts = null)
            where TProfile : Profile, new()
            where TModel : IModel
        {
            var mapper = MappingProfile.CreateMapper<TProfile>();
            if (opts == null)
                return mapper.Map<IEntity, TModel>(entity);
            else
                return mapper.Map(entity, opts);
        }

        public static IEnumerable<TModel> ToModel<TProfile, TModel>(
            this IEnumerable<IEntity> entities,
            Action<IMappingOperationOptions<IEnumerable<IEntity>, IEnumerable<TModel>>> opts = null
        )
            where TProfile : Profile, new()
            where TModel : IModel
        {
            var mapper = MappingProfile.CreateMapper<TProfile>();
            if (opts == null)
                return mapper.Map<IEnumerable<IEntity>, IEnumerable<TModel>>(entities);
            else
                return mapper.Map(entities, opts);
        }

        #endregion

        #region Copy Entity to Entity

        public static TEntity CopyToEntity<TEntity>(this TEntity entity, TEntity destination, Action<IMappingOperationOptions<TEntity, TEntity>> opts = null)
            where TEntity : IEntity
        {
            var mapper = MappingProfile.CreateMapper<EntityMappingProfile<TEntity>>();
            if (opts == null)
                return mapper.Map(entity, destination);
            else
                return mapper.Map(entity, destination, opts);
        }

        public static IEnumerable<TEntity> CopyToEntity<TEntity>(
            this IEnumerable<TEntity> entity,
            IEnumerable<TEntity> destination,
            Action<IMappingOperationOptions<IEnumerable<TEntity>, IEnumerable<TEntity>>> opts = null
        )
            where TEntity : IEntity
        {
            var mapper = MappingProfile.CreateMapper<EntityMappingProfile<TEntity>>();
            if (opts == null)
                return mapper.Map(entity, destination);
            else
                return mapper.Map(entity, destination, opts);
        }

        #endregion

        #region Copy Model to Model

        public static TModel CopyToModel<TModel>(this TModel model, TModel destination, Action<IMappingOperationOptions<TModel, TModel>> opts = null)
            where TModel : IModel
        {
            var mapper = MappingProfile.CreateMapper<ModelMappingProfile<TModel>>();
            if (opts == null)
                return mapper.Map(model, destination);
            else
                return mapper.Map(model, destination, opts);
        }

        public static IEnumerable<TModel> CopyToModel<TModel>(
            this IEnumerable<TModel> entity,
            IEnumerable<TModel> destination,
            Action<IMappingOperationOptions<IEnumerable<TModel>, IEnumerable<TModel>>> opts = null
        )
            where TModel : IModel
        {
            var mapper = MappingProfile.CreateMapper<ModelMappingProfile<TModel>>();
            if (opts == null)
                return mapper.Map(entity, destination);
            else
                return mapper.Map(entity, destination, opts);
        }

        #endregion

        public static string ToJson<TModel>(this TModel model)
            where TModel : IModel
        {
            return JsonSerializer.Serialize(model);
        }

        public static TEntity EnrichEntity<TEntity>(this IMapper mapper, TEntity source, TEntity destination, Action<IMappingOperationOptions> opts = default)
            where TEntity : IEntity
        {
            if (opts != null)
                return mapper.Map(source, destination, opts);
            else
                return mapper.Map(source, destination);
        }
    }
}
