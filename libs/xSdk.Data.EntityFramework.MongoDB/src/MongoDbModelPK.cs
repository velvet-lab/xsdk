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

using MongoDB.Bson;
using xSdk.Data.Converters.Mapper;

namespace xSdk.Data;

public sealed class MongoDbModelPK : PrimaryKey<string>
{
    private readonly object _syncObject = new();

    public MongoDbModelPK()
        : base(ObjectId.GenerateNewId().ToString()) { }

    public MongoDbModelPK(ObjectId initialValue)
        : base(initialValue.ToString()) { }

    public MongoDbModelPK(string intialValue)
        : base(ObjectId.Parse(intialValue).ToString()) { }

    protected override TType Convert<TType>(object value)
    {
        lock (_syncObject)
        {
            if (ObjectIdConverter.TryConvert(value, out string resultAsString))
            {
                if (typeof(TType) == typeof(ObjectId))
                {
                    return (TType)(object)ObjectId.Parse(resultAsString);
                }
                else if (typeof(TType) == typeof(string))
                {
                    return (TType)(object)resultAsString;
                }
            }
            else if (ObjectIdConverter.TryConvert(value, out ObjectId result))
            {
                if (typeof(TType) == typeof(ObjectId))
                {
                    return (TType)(object)result;
                }
                else if (typeof(TType) == typeof(string))
                {
                    return (TType)(object)result.ToString();
                }
            }
        }
        return default;
    }
}
