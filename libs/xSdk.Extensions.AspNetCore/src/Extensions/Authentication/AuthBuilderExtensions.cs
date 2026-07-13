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

using Microsoft.AspNetCore.Authentication;

namespace xSdk.Extensions.Authentication;

public static class AuthBuilderExtensions
{
    extension(AuthBuilder builder)
    {
        public AuthBuilder WithAuthentication(Action<AuthenticationBuilder> configure)
        {
            builder.ConfigureAuthenticationAction = configure;
            return builder;
        }

        public AuthBuilder WithAuthorization(Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions> configure)
        {
            builder.ConfigureAuthorizationAction = configure;
            return builder;
        }

        public AuthBuilder WithSchemeSelector(Func<Microsoft.AspNetCore.Http.HttpContext, string?> selector)
        {
            builder.SelectAuthenticationSchemeAction = selector;
            return builder;
        }
    }
}
