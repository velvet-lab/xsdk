using LiteDB.Async;
using NLog;

namespace xSdk.Data
{
    public partial class NoSqlRepository<TEntity> : Repository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly object _syncObject = new();
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private async Task<TResult> ExecuteInternalAsync<TResult>(
            Func<ILiteCollectionAsync<TEntity>, Task<TResult>> func,
            bool withTransaction,
            CancellationToken token
        )
        {
            TResult result = default;
            ILiteDatabaseAsync openedDatabase = null;
            ILiteDatabaseAsync transactionDatabase = null;
            ILiteCollectionAsync<TEntity> col = null;
            string collectionName = default;

            try
            {
                collectionName = this.GetTableName();
                openedDatabase = this.Database.Open<LiteDatabaseAsync>(true);

                if (withTransaction)
                {
                    transactionDatabase = await openedDatabase.BeginTransactionAsync();
                    col = transactionDatabase.GetCollection<TEntity>(collectionName);
                }
                else
                    col = openedDatabase.GetCollection<TEntity>(collectionName);

                result = await func(col);

                if (withTransaction)
                {
                    if (transactionDatabase != null)
                        await transactionDatabase.CommitAsync();

                    //if (openedDatabase != null)
                    //    await openedDatabase.CheckpointAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "A Error occurred while execute a Operation with Transactionon the Database");

                if (withTransaction && transactionDatabase != null)
                    await transactionDatabase.RollbackAsync();
            }

            return result;
        }

        protected override void Initialize()
        {
            lock (_syncObject)
            {
                try
                {
                    var database = this.Database.Open<LiteDatabaseAsync>(true);
                    var collectionName = GetTableName();
                    database.UnderlyingDatabase.UpdateIndicies<TEntity>(collectionName);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Indicies could not updated for Entity '{0}'", typeof(TEntity).FullName);
                }
            }
        }
    }
}
