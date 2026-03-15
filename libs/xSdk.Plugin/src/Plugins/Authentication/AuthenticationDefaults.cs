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

using AspNetCore.Authentication.ApiKey;

namespace xSdk.Plugins.Authentication;

public static class AuthenticationDefaults
{
    internal const string DefaultScheme = "NotConfigured";

    public static class ApiKeyAuth
    {
        public const string Name = "API Key Authentication";

        public static class InHeader
        {
            public const string Header = "X-API-KEY";
            public const string Scheme = $"{ApiKeyDefaults.AuthenticationScheme}InHeaderScheme";
        }

        public static class InAuthorizationHeader
        {
            public const string Header = "ApiKeyName";
            public const string Scheme = $"{ApiKeyDefaults.AuthenticationScheme}InAuthorizationHeaderScheme";
        }
    }

    public static class MulitAuth
    {
        public const string Scheme = "MultiAuthScheme";
    }
}
