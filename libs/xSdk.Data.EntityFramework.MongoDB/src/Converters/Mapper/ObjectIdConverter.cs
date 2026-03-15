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

namespace xSdk.Data.Converters.Mapper;

public static class ObjectIdConverter
{
    public static string Convert(ObjectId sourceMember)
    {
        if (TryConvert(sourceMember, out string result))
        {
            return result;
        }
        return default;
    }
    public static ObjectId Convert(string sourceMember)
    {
        if (TryConvert(sourceMember, out ObjectId result))
        {
            return result;
        }
        return default;
    }

    internal static bool TryConvert(object value, out string converted)
    {
        converted = default;
        if (value != null && value is ObjectId objectIdValue)
        {
            converted = objectIdValue.ToString().Trim();
            return true;
        }
        return false;
    }

    internal static bool TryConvert(object value, out ObjectId converted)
    {
        converted = default;
        if (value != null && value is string stringValue)
        {
            converted = ObjectId.Parse(stringValue.Trim());
            return true;
        }
        return false;
    }
}
