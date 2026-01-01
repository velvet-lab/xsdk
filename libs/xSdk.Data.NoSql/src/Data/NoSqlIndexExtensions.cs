using LiteDB;

namespace xSdk.Data
{
    internal static class NoSqlIndexExtensions
    {
        internal static void UpdateIndicies<TEntity>(this ILiteDatabase db, string collectionName)
            where TEntity : IEntity
        {
            var collection = db.GetCollection(collectionName);

            var declaredIndicies = LoadDeclaredIndicies<TEntity>();
            var existingIndicies = GetIndexes(db);

            foreach (var index in declaredIndicies)
            {
                var existingIndex = existingIndicies.SingleOrDefault(x => string.Compare(x.Field, index.Field, StringComparison.OrdinalIgnoreCase) == 0);
                if (existingIndex == null)
                {
                    if (!string.IsNullOrEmpty(index.Expression))
                        collection.EnsureIndex(index.Field, index.Expression, index.IsUnique);
                    else
                        collection.EnsureIndex(index.Field, index.IsUnique);
                }
            }
        }

        private static List<NoSqlIndex> LoadDeclaredIndicies<TEntity>()
            where TEntity : IEntity
        {
            var result = new List<NoSqlIndex>();
            var entityType = typeof(TEntity);
            foreach (var property in entityType.GetProperties())
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(NoSqlIndexAttribute)) as NoSqlIndexAttribute;
                if (attribute != null)
                {
                    var index = attribute.Index;
                    index.Field = property.Name;
                    result.Add(index);
                }
            }

            foreach (var field in entityType.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(NoSqlIndexAttribute)) as NoSqlIndexAttribute;
                if (attribute != null)
                {
                    attribute.Index.Field = field.Name;
                    result.Add(attribute.Index);
                }
            }

            return result;
        }

        private static IEnumerable<NoSqlIndex> GetIndexes(ILiteDatabase database)
        {
            var reader = database.Execute("SELECT * FROM $INDEXES");
            var result = new List<NoSqlIndex>();

            foreach (var entry in reader.Current.AsDocument["expr"].AsArray)
            {
                result.Add(
                    new NoSqlIndex
                    {
                        Expression = entry["expression"],
                        Field = entry["name"],
                        IsUnique = entry["unique"],
                    }
                );
            }
            return result;
        }
    }
}
