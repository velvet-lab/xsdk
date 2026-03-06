using JsonFlatFileDataStore;
using System.Linq.Expressions;

namespace xSdk.Data
{
    public abstract class FlatFileRepository<TEntity> : Repository<TEntity>
        where TEntity : class, IEntity
    {
        public override Task<bool> InsertAsync(TEntity entity, CancellationToken token = default) =>
            ExecuteInternalAsync((col) => col.InsertOneAsync(entity), token);

        public override Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default) =>
            ExecuteInternalAsync((col) => col.InsertManyAsync(entities).ContinueWith(task => entities.Count()), token);

        public override Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default) =>
            ExecuteInternalAsync((col) => col.DeleteOneAsync(x => x.PrimaryKey == primaryKey), token);

        public override Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default) =>
            ExecuteInternalAsync((col) => col.DeleteOneAsync(x => x.PrimaryKey == entity.PrimaryKey), token);

        public override Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
        {
            return ExecuteInternalAsync(
                async (col) =>
                {
                    var removed = 0;
                    foreach (var entity in entities)
                    {
                        var result = await col.DeleteOneAsync(entity.PrimaryKey);
                        if (result)
                            removed++;
                    }
                    return removed;
                },
                token
            );
        }

        protected Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
            ExecuteInternalAsync(col => col.DeleteManyAsync(ConvertFilter(filter)), token);

        public override Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default) =>
            ExecuteInternalAsync((col) => Task.FromResult(col.AsQueryable().SingleOrDefault(x => x.PrimaryKey == primaryKey)), token);

        public override Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default) =>
            ExecuteInternalAsync((col) => Task.FromResult(col.AsQueryable()), token);

        protected TEntity? Select(Expression<Func<TEntity, bool>> filter) => SelectAsync(filter).GetAwaiter().GetResult();

        protected Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
        {
            return ExecuteInternalAsync(col => Task.FromResult(col.Find(ConvertFilter(filter))), token)
                .ContinueWith(task => task.Result.SingleOrDefault(), token);
        }

        protected IEnumerable<TEntity> SelectList(Expression<Func<TEntity, bool>> filter) => SelectListAsync(filter).GetAwaiter().GetResult();

        protected Task<IEnumerable<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
            ExecuteInternalAsync(col => Task.FromResult(col.Find(ConvertFilter(filter))), token);

        public override Task<bool> UpdateAsync(IPrimaryKey primaryKey, TEntity entity, CancellationToken token = default) =>
            ExecuteInternalAsync((col) => col.UpdateOneAsync(entity.PrimaryKey.GetValue<object>(), entity), token);

        public override Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default)
        {
            return ExecuteInternalAsync(
                async (col) =>
                {
                    var item = col.AsQueryable().SingleOrDefault(x => x.PrimaryKey == entity.PrimaryKey);

                    var result = false;
                    if (item == null)
                        result = await col.InsertOneAsync(entity);
                    else
                        result = await col.UpdateOneAsync(entity.PrimaryKey, entity);

                    return result;
                },
                token
            );
        }

        private async Task<TResult?> ExecuteInternalAsync<TResult>(Func<IDocumentCollection<TEntity>, Task<TResult>> func, CancellationToken token)
        {
            TResult? result = default;
            IDataStore? openedDatabase = null;
            IDocumentCollection<TEntity>? col = null;
            string? collectionName = default;

            try
            {
                collectionName = this.GetTableName();
                openedDatabase = this.Database.Open<IDataStore>();

                col = openedDatabase.GetCollection<TEntity>(collectionName);

                result = await func(col);
            }
            catch (Exception ex)
            {
                throw new SdkException("A Error occurred while execute a Operation for the Database", ex);
            }

            return result;
        }

        private Predicate<TEntity> ConvertFilter(Expression<Func<TEntity, bool>> filter)
        {
            var compiled = filter.Compile();
            Predicate<TEntity> pred = x => compiled(x);

            return pred;
        }
    }
}
