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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace xSdk.Extensions.Links;

file class TestController : ControllerBase
{
    public TestController(HttpContext httpContext)
    {
        ControllerContext = new ControllerContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData(),
            ActionDescriptor = new ControllerActionDescriptor()
        };
    }
}

public class HateoasExtensionsTests
{
    [Fact]
    public void IsHateoasEnabled_WithTrueHeader_ReturnsTrue()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-HATEOAS-ENABLED", "true");
        var controller = new TestController(httpContext);

        var result = controller.IsHateoasEnabled();

        Assert.True(result);
    }

    [Fact]
    public void IsHateoasEnabled_WithOneHeader_ReturnsTrue()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-HATEOAS-ENABLED", "1");
        var controller = new TestController(httpContext);

        var result = controller.IsHateoasEnabled();

        Assert.True(result);
    }

    [Fact]
    public void IsHateoasEnabled_WithFalseHeader_ReturnsFalse()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Append("X-HATEOAS-ENABLED", "false");
        var controller = new TestController(httpContext);

        var result = controller.IsHateoasEnabled();

        Assert.False(result);
    }

    [Fact]
    public void IsHateoasEnabled_WithoutHeader_ReturnsFalse()
    {
        var httpContext = new DefaultHttpContext();
        var controller = new TestController(httpContext);

        var result = controller.IsHateoasEnabled();

        Assert.False(result);
    }
}
