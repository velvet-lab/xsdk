using System.ComponentModel.DataAnnotations.Schema;
using xSdk.Extensions.Variable;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Data;

public abstract class Repository : IRepository
{
    private IDatabase _database;

    #region Dispose Handling

    ~Repository() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Database.Close();
    }

    #endregion

    internal IEnumerable<InternalDatabaseSetup> InternalSetups { get; set; }

    protected bool IsDemoMode
    {
        get
        {
            var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
            if (envSetup != null)
            {
                return envSetup.IsDemo;
            }
            return false;
        }
    }

    internal void Configure(IDatabase database)
    {
        _database = database;
        Initialize();
    }

    protected virtual IDatabase Database
    {
        get
        {
            if (_database == null)
                throw new SdkException("Repository was not initialzed with the Datalayer Factory. So no Database is loaded.");

            return _database;
        }
    }

    protected virtual void Initialize() { }

    protected TKey ConvertTo<TKey>(object key) => TypeConverter.ConvertTo<TKey>(key);
}

public abstract class Repository<TEntity> : Repository, IRepository<TEntity>
    where TEntity : class, IEntity
{
    private Repository<TEntity>? _fakeRepository;

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

    protected TKey ConvertTo<TKey>(TEntity entity) => ConvertTo<TKey>(entity.PrimaryKey);

    public virtual bool Insert(TEntity entity) => InsertAsync(entity).GetAwaiter().GetResult();

    public abstract Task<bool> InsertAsync(TEntity entity, CancellationToken token = default);

    public virtual int Insert(IEnumerable<TEntity> entities) => InsertAsync(entities).GetAwaiter().GetResult();

    public abstract Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    public virtual bool Remove(IPrimaryKey primaryKey) => RemoveAsync(primaryKey).GetAwaiter().GetResult();

    public abstract Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default);

    public int Remove(IEnumerable<IPrimaryKey> primaryKeys) => RemoveAsync(primaryKeys).GetAwaiter().GetResult();

    public abstract Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default);

    public virtual bool Remove(TEntity entity) => RemoveAsync(entity).GetAwaiter().GetResult();

    public abstract Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default);

    public virtual int Remove(IEnumerable<TEntity> entities) => RemoveAsync(entities).GetAwaiter().GetResult();

    public abstract Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

    public virtual TEntity? Select(IPrimaryKey primaryKey) => SelectAsync(primaryKey).GetAwaiter().GetResult();

    public abstract Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default);

    public virtual IEnumerable<TEntity> SelectList() => SelectListAsync().GetAwaiter().GetResult();

    public abstract Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default);

    public virtual bool Update(IPrimaryKey primaryKey, TEntity entity) => UpdateAsync(primaryKey, entity).GetAwaiter().GetResult();

    public abstract Task<bool> UpdateAsync(IPrimaryKey primaryKey, TEntity entity, CancellationToken token = default);

    public virtual bool Upsert(TEntity entity) => UpsertAsync(entity).GetAwaiter().GetResult();

    public abstract Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default);

    protected virtual Task<IEnumerable<TEntity>> CreateFakesAsync(CancellationToken token = default) =>
        Task.FromResult<IEnumerable<TEntity>>(new List<TEntity>());

    protected Task<TResult> ExecuteAsDemoIfEnabledAsync<TResult>(Func<Repository<TEntity>, Task<TResult>> concreteCall, CancellationToken token = default)
    {
        if (IsDemoMode)
        {
            if (_fakeRepository == null)
            {
                var items = CreateFakesAsync(token).GetAwaiter().GetResult();
                _fakeRepository = new FakeRepository<TEntity>(items);
            }

            return concreteCall(_fakeRepository);
        }
        return concreteCall(this);
    }
}
