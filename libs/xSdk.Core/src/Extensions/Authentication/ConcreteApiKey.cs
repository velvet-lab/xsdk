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

using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;
using xSdk.Data;

namespace xSdk.Extensions.Authentication;

public sealed class ConcreteApiKey : IApiKey
{
    private readonly IReadOnlyCollection<Claim> _claims;

    public ConcreteApiKey()
    {
        _claims = new List<Claim>();
    }

    public string Key { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public IReadOnlyCollection<Claim> Claims { get; set; } = new List<Claim>();
}
