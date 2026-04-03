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

public sealed class GuidPK : PrimaryKey<Guid>
{
    private readonly object _syncObject = new();

    public GuidPK()
        : base(Guid.NewGuid()) { }

    public GuidPK(Guid initialValue)
        : base(initialValue) { }

    public GuidPK(string initialValue)
        : base(Guid.Parse(initialValue)) { }

    protected override TType Convert<TType>(object value)
    {
        lock (_syncObject)
        {
            if (GuidConverter.TryConvert(value, out Guid result))
            {
                return TypeConverter.ConvertTo<TType>(result);
            }
            else if (GuidConverter.TryConvert(value, out string resultString))
            {
                return TypeConverter.ConvertTo<TType>(resultString);
            }
        }

        return default;
    }
}
