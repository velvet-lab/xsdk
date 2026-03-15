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

using xSdk.Data.Converters.Mapper;
using xSdk.Shared;

namespace xSdk.Data;

public sealed class GuidStringPK : PrimaryKey<string>
{
    private static readonly object _syncObject = new();

    public GuidStringPK()
        : base(Guid.NewGuid().ToString()) { }

    public GuidStringPK(Guid initialValue)
        : base(initialValue.ToString()) { }

    public GuidStringPK(string initialValue)
        : base(Guid.Parse(initialValue).ToString()) { }

    protected override TType Convert<TType>(object value)
    {
        lock (_syncObject)
        {
            if (GuidConverter.TryConvert(value, out string stringResult))
            {
                return TypeConverter.ConvertTo<TType>(stringResult);
            }
            else if (GuidConverter.TryConvert(value, out Guid guidResult))
            {
                return TypeConverter.ConvertTo<TType>(guidResult);
            }
        }

        return default;
    }
}
