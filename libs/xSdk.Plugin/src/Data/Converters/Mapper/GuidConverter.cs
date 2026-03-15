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

using System.ComponentModel;

namespace xSdk.Data.Converters.Mapper;

public static class GuidConverter
{
    public static Guid Convert(string value)
    {
        if (TryConvert(value, out Guid result))
        {
            return result;
        }

        return default;
    }

    public static string Convert(Guid value)
    {
        if (TryConvert(value, out string result))
        {
            return result;
        }

        return default;
    }

    internal static bool TryConvert(object value, out Guid converted)
    {
        converted = default;
        if (value != null && value is string stringValue)
        {
            converted = Guid.Parse(stringValue);
            return true;
        }
        return false;
    }

    internal static bool TryConvert(object value, out string converted)
    {
        converted = default;
        if (value != null && value is Guid guidValue)
        {
            converted = guidValue.ToString();
            return true;
        }
        return false;
    }
}
