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

using LiteDB;

namespace xSdk.Data;

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
