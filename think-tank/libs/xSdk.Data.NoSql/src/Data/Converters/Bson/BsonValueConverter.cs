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

namespace xSdk.Data.Converters.Bson;

public static class BsonValueConverter
{
    public static BsonValue Convert(object value)
    {
        if (value is bool)
            return new BsonValue(System.Convert.ToBoolean(value));
        else if (value is int)
            return new BsonValue(System.Convert.ToInt32(value));
        else if (value is decimal)
            return new BsonValue(System.Convert.ToDecimal(value));
        else if (value is double)
            return new BsonValue(System.Convert.ToDouble(value));
        else if (value is Guid)
            return new BsonValue(Guid.Parse(value.ToString()));
        else if (value is long)
            return new BsonValue(System.Convert.ToInt64(value));
        else if (value is string)
            return new BsonValue(System.Convert.ToString(value));
        else if (value is ObjectId)
            return new BsonValue(value as ObjectId);
        else if (value is DateTime)
            return new BsonValue(DateTime.Parse(value.ToString()));
        else
            throw new SdkException("Value is not a convertable Bson Value");
    }
}
