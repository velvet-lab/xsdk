using xSdk.Data.Converters.Bson;

namespace xSdk.Data;

public partial class NoSqlRepository<TEntity>
{
    public override Task<bool> UpsertAsync(TEntity entity, CancellationToken token = default) =>
        ExecuteInternalAsync(col => col.UpsertAsync(BsonValueConverter.Convert(entity.PrimaryKey), entity), true, token);
}
