using xSdk.Data.Converters.Bson;
using LiteDB;
using System.Linq.Expressions;

namespace xSdk.Data
{
    public partial class NoSqlRepository<TEntity>
    {
        public override Task<TEntity?> SelectAsync(IPrimaryKey primaryKey, CancellationToken token = default)
        {
            _logger.Trace("Get Entity by '{0}'...", primaryKey);
            return ExecuteInternalAsync(col => col.FindByIdAsync(BsonValueConverter.Convert(primaryKey.GetValue<ObjectId>())), false, token);
        }

        public override Task<IEnumerable<TEntity>> SelectListAsync(CancellationToken token = default)
        {
            _logger.Trace("Get all Entities ...");
            return ExecuteInternalAsync(col => col.FindAllAsync(), false, token);
        }

        protected TEntity Select(Expression<Func<TEntity, bool>> filter) => SelectAsync(filter).GetAwaiter().GetResult();

        protected Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
        {
            _logger.Trace("Get Entity");
            return ExecuteInternalAsync(col => col.FindAllAsync(), false, token).ContinueWith(task => task.Result.SingleOrDefault(filter.Compile()), token);
        }

        protected IEnumerable<TEntity> SelectList(Expression<Func<TEntity, bool>> filter) => SelectListAsync(filter).GetAwaiter().GetResult();

        protected Task<IEnumerable<TEntity>> SelectListAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default)
        {
            _logger.Trace("Get Entities by predicate");

            return ExecuteInternalAsync(col => col.FindAllAsync(), false, token)
                .ContinueWith(
                    task =>
                    {
                        IEnumerable<TEntity> result = task.Result.Where(filter.Compile()).ToList();
                        return result;
                    },
                    token
                );
        }
    }
}
