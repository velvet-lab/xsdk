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

using System.Collections.ObjectModel;
using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Authentication;
using xSdk.Security.Claims;

namespace xSdk.Demos.Builders;

internal class ApiKeyHandlerTwo : IApiKeyHandler
{
    public ApiKeyHandlerTwo(ILogger<ApiKeyHandlerTwo> logger)
    {
        _logger = logger;
    }


    public Task<IApiKey?> GetApiKeyAsync(string key)
    {
        return Task.FromResult(_apiKeys.SingleOrDefault(x => string.Compare(x.Key, key) == 0));
    }

    // List of ApiKeys only for Demo Purposes. Its highly recommended to host this in a external service
    private readonly List<IApiKey> _apiKeys = new List<IApiKey>
    {
        new ConcreteApiKey
        {
            Key = "1bee9671-13fa-489a-a35e-506fec8f74a1",
            OwnerName = "Only Read user",
            Claims = new ReadOnlyCollection<Claim>(new[] { ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Read) }),
        },
        new ConcreteApiKey
        {
            Key = "0d3d3058-e59e-4178-ac23-0ed89168a925",
            OwnerName = "Write User",
            Claims = new ReadOnlyCollection<Claim>(
                new[]
                {
                    ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Read),
                    ClaimCreator.CreateClaim(MyClaimTypes.MyTableA.Permission, MyClaimValues.Permissions.Write),
                }
            ),
        },
    };
    private readonly ILogger<ApiKeyHandlerTwo> _logger;
}
