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
using xSdk.Data.Converters.Mapper;

namespace xSdk.Data;

internal class NoSqlEntityPK : PrimaryKey<ObjectId>
{
    private readonly object _syncObject = new();

    public NoSqlEntityPK()
        : base(ObjectId.NewObjectId()) { }

    public NoSqlEntityPK(ObjectId initialValue)
        : base(initialValue) { }

    public NoSqlEntityPK(string intialValue)
        : base(new ObjectId(intialValue)) { }

    protected override TType Convert<TType>(object value)
    {
        lock (_syncObject)
        {
            if (ObjectIdConverter.TryConvert(value, out ObjectId result))
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
            else if (ObjectIdConverter.TryConvert(value, out string resultString))
            {
                if (typeof(TType) == typeof(ObjectId))
                {
                    return (TType)(object)new ObjectId(resultString);
                }
                else if (typeof(TType) == typeof(string))
                {
                    return (TType)(object)resultString;
                }
            }
        }
        return default;
    }
}
