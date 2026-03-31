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

using xSdk.Security;

namespace xSdk.Security;

public class SecurityContextTests
{
    [Fact]
    public void IsSuperUser_DoesNotThrow()
    {
        var ex = Record.Exception(() => SecurityContext.IsSuperUser());

        Assert.Null(ex);
    }

    [Fact]
    public void IsSuperUser_ReturnsBoolean()
    {
        var result = SecurityContext.IsSuperUser();

        Assert.IsType<bool>(result);
    }
}
