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
using Microsoft.Extensions.Logging;
using xSdk.Data.Converters.Bson;

namespace xSdk.Data;

public partial class NoSqlRepository<TEntity>
{
    //public override int Execute(string command, Dictionary<string, object> parameters)
    //{
    //    int result = 0;
    //    try
    //    {
    //        var reader = CreateReader(command, parameters);
    //        while (reader.Read())
    //        {
    //            result++;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.Verbose(ex, "A Error occurred while execute a Command on the Database");
    //    }

    //    return result;
    //}

    //public override TReader ExecuteReader<TReader>(string command, Dictionary<string, object> parameters)
    //{
    //    var reader = CreateReader(command, parameters);

    //    return (TReader) reader;
    //}

    //public override object ExecuteScalar(string command, Dictionary<string, object> parameters)
    //{
    //    throw new AminOOException("Execute Scalar is not available for NoSql Data Repositories. Give ExecuteReader a try");
    //}

    //public override TValue ExecuteScalar<TValue>(string command, Dictionary<string, object> parameters)
    //{
    //    throw new AminOOException("Execute Scalar is not available for NoSql Data Repositories. Give ExecuteReader a try");
    //}

    //public override Task<int> ExecuteAsync(string command, Dictionary<string, object> parameters)
    //{
    //    throw new AminOOException("Async Execute is not available for NoSql Data Repositories");
    //}

    //public override Task<TReader> ExecuteReaderAsync<TReader>(string command, Dictionary<string, object> parameters)
    //{
    //    throw new AminOOException("Async Execute is not available for NoSql Data Repositories");
    //}

    //public override Task<object> ExecuteScalarAsync(string command, Dictionary<string, object> parameters)
    //{
    //    throw new AminOOException("Async Execute is not available for NoSql Data Repositories");
    //}

    //public override Task<TValue> ExecuteScalarAsync<TValue>(string command, Dictionary<string, object> parameters)
    //{
    //    throw new AminOOException("Async Execute is not available for NoSql Data Repositories");
    //}

    private IBsonDataReader CreateReader(string command, Dictionary<string, object> parameters)
    {
        IBsonDataReader result = default;
        try
        {
            var name = this.GetTableName();
            var openedDatabase = this.Database.Open<LiteDatabase>();

            var bsonArgs = parameters.ToDictionary(x => x.Key, y => BsonValueConverter.Convert(y.Value));
            var bsonDoc = new BsonDocument(bsonArgs);
            result = openedDatabase.Execute(command, bsonDoc);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "A Error occurred while execute a Command the Database");
        }

        return result;
    }
}
