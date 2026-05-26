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

using Microsoft.AspNetCore.Mvc;

namespace xSdk.Extensions.Links;

public static class HateoasExtensions
{
    public static bool IsHateoasEnabled(this ControllerBase controller)
    {
        if (controller.HttpContext.Request.Headers.TryGetValue("X-HATEOAS-ENABLED", out var values))
        {
            if (values.ToString().ToLower() == "true" || values.ToString() == "1")
            {
                //
                return true;
            }
        }
        return false;
    }
}
