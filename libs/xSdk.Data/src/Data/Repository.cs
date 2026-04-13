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

using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Bogus.DataSets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using xSdk.Extensions.Options;
using xSdk.Extensions.Variable;
using xSdk.Shared;

namespace xSdk.Data;

public abstract class Repository : IRepository
{
    public string? DatalayerName { get; internal set; }

    public IServiceProvider? Services { get; internal set; }

    protected bool IsDemoMode
    {
        get
        {
            var options = GetOptions<EnvironmentOptions>(OptionsScope.Default);
            if (options != null)
            {
                return options.IsDemo;
            }
            return false;
        }
    }

    protected IDatabaseHandler? DatabaseHandler => Services?.GetRequiredKeyedService<IDatabaseHandler>(DatalayerName);

    protected TOptions? GetOptions<TOptions>(OptionsScope scope = OptionsScope.Default)
    {
        var options = Services?.GetService<IOptionsMonitor<TOptions>>();
        if (options != null)
        {
            if (scope == OptionsScope.Datalayer)
            {
                return options.Get(DatalayerName);
            }
            else
            {
                return options.CurrentValue;
            }
        }
        return default;
    }
}

public abstract class Repository<TEntity, TPrimaryKeyType> : Repository, IRepository<TEntity, TPrimaryKeyType>
    where TEntity : class, IEntity<TPrimaryKeyType>
{
    private Repository<TEntity, TPrimaryKeyType>? _fakeRepository;

    protected string GetTableName()
    {
        var entityType = typeof(TEntity);
        var name = GetTableNameFromType(entityType);

        if (string.IsNullOrEmpty(name))
        {
            var repoType = GetType();
            name = GetTableNameFromType(repoType);

            if (string.IsNullOrEmpty(name))
            {
                name = repoType.Name;
                name = name.Replace("Repository", "");
                name = name.Replace("Store", "");
            }
        }

        return name;
    }

    private string GetTableNameFromType(Type type)
    {
        string name = default;
        if (Attribute.GetCustomAttribute(type, typeof(TableAttribute)) is TableAttribute attribute)
            name = attribute.Name;

        return name;
    }

    public virtual bool Insert(TEntity entity) => InsertAsync(entity).GetAwaiter().GetResult();

    public abstract Task<bool> InsertAsync(TEntity entity, CancellationToken token = default);

    public virtual int Insert(IEnumerable<TEntity> entities) => InsertAsync(entities).GetAwaiter().GetResult();

    public abstract Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    public virtual bool Remove(TPrimaryKeyType primaryKey) => RemoveAsync(primaryKey).GetAwaiter().GetResult();

    public abstract Task<bool> RemoveAsync(TPrimaryKeyType primaryKey, CancellationToken token = default);

    public int Remove(IEnumerable<TPrimaryKeyType> primaryKeys) => RemoveAsync(primaryKeys).GetAwaiter().GetResult();

    public abstract Task<int> RemoveAsync(IEnumerable<TPrimaryKeyType> primaryKeys, CancellationToken token = default);

    public virtual bool Remove(TEntity entity) => RemoveAsync(entity).GetAwaiter().GetResult();

    public abstract Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default);

    public virtual int Remove(IEnumerable<TEntity> entities) => RemoveAsync(entities).GetAwaiter().GetResult();

    public abstract Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    public virtual TEntity? Select(TPrimaryKeyType primaryKey) => SelectAsync(primaryKey).GetAwaiter().GetResult();

    public abstract Task<TEntity?> SelectAsync(TPrimaryKeyType primaryKey, CancellationToken token = default);

    public virtual IEnumerable<TEntity> SelectList() => SelectListAsync().GetAwaiter().GetResult();

    public abstract Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default);

    public virtual bool Update(TPrimaryKeyType primaryKey, TEntity entity) => UpdateAsync(primaryKey, entity).GetAwaiter().GetResult();

    public abstract Task<bool> UpdateAsync(TPrimaryKeyType primaryKey, TEntity entity, CancellationToken token = default);

    public virtual bool Upsert(TEntity entity) => UpsertAsync(entity).GetAwaiter().GetResult();

    public abstract Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default);

    protected virtual Task<IEnumerable<TEntity>> CreateFakesAsync(CancellationToken token = default) =>
        Task.FromResult<IEnumerable<TEntity>>(new List<TEntity>());

    protected Task<TResult> ExecuteAsDemoIfEnabledAsync<TResult>(Func<Repository<TEntity, TPrimaryKeyType>, Task<TResult>> concreteCall, CancellationToken token = default)
    {
        if (IsDemoMode)
        {
            if (_fakeRepository == null)
            {
                var items = CreateFakesAsync(token).GetAwaiter().GetResult();
                _fakeRepository = new FakeRepository<TEntity, TPrimaryKeyType>(items);
            }

            return concreteCall(_fakeRepository);
        }
        return concreteCall(this);
    }
}
