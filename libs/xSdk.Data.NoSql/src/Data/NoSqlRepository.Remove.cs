using xSdk.Data.Converters.Bson;
using LiteDB;
using System.Linq.Expressions;

namespace xSdk.Data
{
    public partial class NoSqlRepository<TEntity>
    {
        public override Task<bool> RemoveAsync(IPrimaryKey primaryKey, CancellationToken token = default)
        {
            _logger.Trace("Remove Entity '{0}'", primaryKey);
            return ExecuteInternalAsync(col => col.DeleteAsync(BsonValueConverter.Convert(primaryKey.GetValue<ObjectId>())), true, token);
        }

        public override Task<bool> RemoveAsync(TEntity entity, CancellationToken token = default) => RemoveAsync(entity.PrimaryKey, token);

        public override async Task<int> RemoveAsync(IEnumerable<IPrimaryKey> primaryKeys, CancellationToken token = default)
        {
            _logger.Trace("Remove Entities ...");

            var deleted = 0;
            foreach (var primaryKey in primaryKeys)
            {
                var result = await RemoveAsync(primaryKey, token);
                if (result)
                    deleted += 1;
            }

            return deleted;
        }

        public override Task<int> RemoveAsync(IEnumerable<TEntity> entities, CancellationToken token = default) =>
            RemoveAsync(entities.Select(x => x.PrimaryKey), token);

        protected Task<int> RemoveAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default) =>
            ExecuteInternalAsync(col => col.DeleteManyAsync(filter), true, token);
    }
}
