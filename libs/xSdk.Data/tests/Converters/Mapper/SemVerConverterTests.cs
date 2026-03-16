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

namespace xSdk.Data.Converters.Mapper;

public class SemVerConverterTests
{
    private readonly SemVer _version = new SemVer("1.2.3");

    private readonly string _versionString = "1.2.3";

    [Fact]
    public void ConvertSemVerToString()
    {
        var actual = SemVerConverter.Convert(_version);

        Assert.NotNull(actual);
        Assert.IsType<string>(actual);
    }

    [Fact]
    public void ConvertStringToSemVer()
    {
        var actual = SemVerConverter.Convert(_versionString);

        Assert.NotNull(actual);
        Assert.IsType<SemVer>(actual);
    }
}
