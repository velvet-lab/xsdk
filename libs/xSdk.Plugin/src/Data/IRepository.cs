namespace xSdk.Data
{
    public interface IRepository : IDisposable { }

    public interface IRepository<TEntity> : IRepository
        where TEntity : class, IEntity
    {
        bool Insert(TEntity entity);

        Task<bool> InsertAsync(TEntity entity, CancellationToken token = default);

        int Insert(IEnumerable<TEntity> entities);

        Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        bool Remove(IPrimaryKey primaryKey);

        Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default);

        int Remove(IEnumerable<IPrimaryKey> primaryKeys);

        Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default);

        bool Remove(TEntity entity);

        Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default);

        int Remove(IEnumerable<TEntity> entities);

        Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        TEntity? Select(IPrimaryKey primaryKey);

        Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default);

        IEnumerable<TEntity> SelectList();

        Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default);

        bool Update(IPrimaryKey primaryKey, TEntity entity);

        Task<bool> UpdateAsync(IPrimaryKey primaryKey, TEntity entity, CancellationToken token = default);

        bool Upsert(TEntity entity);

        Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default);
    }
}
