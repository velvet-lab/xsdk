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
using Microsoft.Extensions.Logging.Abstractions;

namespace xSdk.Controllers;

public class HealthControllerTests
{
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        NullLogger<HealthController> logger = NullLogger<HealthController>.Instance;
        _controller = new HealthController(logger);
    }

    [Fact]
    public async Task GetStatus_ReturnsOkResult()
    {
        ActionResult result = await _controller.GetStatus();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetStatus_ReturnsOkString()
    {
        var result = await _controller.GetStatus() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal("ok", result.Value);
    }

    [Fact]
    public async Task GetPong_ReturnsOkResult()
    {
        ActionResult result = await _controller.GetPong();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetPong_ReturnsOkString()
    {
        var result = await _controller.GetPong() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal("pong", result.Value);
    }
}
