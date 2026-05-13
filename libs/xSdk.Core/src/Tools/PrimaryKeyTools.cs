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

using ComponentModel = System.ComponentModel;

namespace xSdk.Tools;

public static class PrimaryKeyTools
{
    public static TTargetType Generate<TTargetType>()
    {
        Type primaryKeyType = typeof(TTargetType);

        if (primaryKeyType == typeof(Guid))
        {
            return (TTargetType)(object)Guid.NewGuid();
        }
        else if (primaryKeyType == typeof(int))
        {
            Bogus.Randomizer randomizer = new Bogus.Randomizer();
            return (TTargetType)(object)randomizer.Int();
        }
        else if (primaryKeyType == typeof(long))
        {
            Bogus.Randomizer randomizer = new Bogus.Randomizer();
            return (TTargetType)(object)randomizer.Long();
        }

        throw new NotSupportedException($"Automatic generation of primary key for type {typeof(TTargetType)} is not supported.");
    }

    public static TTargetType? Convert<TTargetType>(string? value)
    {
        TTargetType? result = default;

        if (!string.IsNullOrEmpty(value))
        {
            Type targetType = typeof(TTargetType);
            if (targetType == typeof(int))
            {
                result = (TTargetType)(object)System.Convert.ToInt32(value);
            }
            else if (targetType == typeof(long))
            {
                result = (TTargetType)(object)System.Convert.ToInt64(value);
            }
            else if (targetType == typeof(Guid))
            {
                result = (TTargetType)(object)Guid.Parse(value);
            }
            else
            {
                ComponentModel.TypeConverter converter = ComponentModel.TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    result = (TTargetType)converter.ConvertFromString(value)!;
                }
                else
                {
                    throw new InvalidOperationException($"Cannot convert string to {targetType.FullName}");
                }
            }
        }

        return result;
    }

    public static string? Convert<TSourceType>(TSourceType value)
    {
        return value.ToString();
    }
}
