namespace xSdk.Data;

public partial class NoSqlRepository<TEntity>
{
    public override async Task<bool> InsertAsync(TEntity entity, CancellationToken token = default)
    {
        _logger.Trace("Add Entity '{0}'", entity.PrimaryKey);
        var result = await ExecuteInternalAsync(col => col.InsertAsync(entity), true, token);
        return result.RawValue != null;
    }

    public override Task<int> InsertAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        _logger.Trace("Add Entities");
        return ExecuteInternalAsync(col => col.InsertBulkAsync(entities), true, token);
    }
}
