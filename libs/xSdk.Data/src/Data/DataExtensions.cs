/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Text.Json;
using Mapster;
using MapsterMapper;

namespace xSdk.Data;

public static class DataExtensions
{
    #region From Model to Entity

    public static TEntity ToEntity<TProfile, TEntity>(this IModel model)
        where TProfile : MappingProfile, new()
        where TEntity : IEntity
    {
        var mapper = MappingFactory.CreateMapper<TProfile>();

        return mapper.Map<TEntity>(model);
    }

    public static IEnumerable<TEntity> ToEntity<TProfile, TEntity>(this IEnumerable<IModel> models)
        where TProfile : MappingProfile, new()
        where TEntity : IEntity
    {
        var mapper = MappingFactory.CreateMapper<TProfile>();
        return mapper.Map<IEnumerable<TEntity>>(models);
    }

    #endregion

    #region From Entity to Model

    public static TModel ToModel<TProfile, TModel>(this IEntity entity)
        where TProfile : MappingProfile, new()
        where TModel : IModel
    {
        var mapper = MappingFactory.CreateMapper<TProfile>();
        return mapper.Map<TModel>(entity);
    }

    public static IEnumerable<TModel> ToModel<TProfile, TModel>(this IEnumerable<IEntity> entities)
        where TProfile : MappingProfile, new()
        where TModel : IModel
    {
        var mapper = MappingFactory.CreateMapper<TProfile>();

        return mapper.Map<IEnumerable<IEntity>, IEnumerable<TModel>>(entities);
    }

    #endregion

    #region Copy Entity to Entity

    public static TEntity CopyToEntity<TEntity>(this TEntity entity, TEntity destination)
        where TEntity : IEntity
    {
        var mapper = MappingFactory.CreateMapper<EntityMappingProfile<TEntity>>();
        return mapper.Map(entity, destination);
    }

    public static IEnumerable<TEntity> CopyToEntity<TEntity>(this IEnumerable<TEntity> entity, IEnumerable<TEntity> destination)
        where TEntity : IEntity
    {
        var mapper = MappingFactory.CreateMapper<EntityMappingProfile<TEntity>>();
        return mapper.Map(entity, destination);
    }

    #endregion

    #region Copy Model to Model

    public static TModel CopyToModel<TModel>(this TModel model, TModel destination)
        where TModel : IModel
    {
        var mapper = MappingFactory.CreateMapper<ModelMappingProfile<TModel>>();
        return mapper.Map(model, destination);
    }

    public static IEnumerable<TModel> CopyToModel<TModel>(this IEnumerable<TModel> entity, IEnumerable<TModel> destination)
        where TModel : IModel
    {
        var mapper = MappingFactory.CreateMapper<ModelMappingProfile<TModel>>();
        return mapper.Map(entity, destination);
    }

    #endregion

    public static string ToJson<TModel>(this TModel model)
        where TModel : IModel
    {
        return JsonSerializer.Serialize(model);
    }

    public static TEntity EnrichEntity<TEntity>(this IMapper mapper, TEntity source, TEntity destination)
        where TEntity : IEntity
    {
        return mapper.Map(source, destination);
    }
}
