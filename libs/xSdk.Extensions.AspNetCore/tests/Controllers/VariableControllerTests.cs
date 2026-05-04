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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using xSdk.Controllers;
using xSdk.Data.Models;
using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Extensions.AspNetCore.Tests.Controllers;

public class VariableControllerTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    private VariableController CreateController()
    {
        var variableSvc = fixture.BuildHost().Services.GetRequiredService<IVariableService>();
        var logger = NullLogger<HealthController>.Instance;
        return new VariableController(variableSvc, logger);
    }

    [Fact]
    public async Task GetVariables_ReturnsOkResult()
    {
        var controller = CreateController();

        var result = await controller.GetVariables();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetVariables_ReturnsListOfVariableModels()
    {
        var controller = CreateController();

        var result = await controller.GetVariables();
        var ok = result.Result as OkObjectResult;

        Assert.NotNull(ok);
        Assert.IsAssignableFrom<IEnumerable<VariableModel>>(ok.Value);
    }

    [Fact]
    public async Task GetVariable_WithValidName_ReturnsOkResult()
    {
        var controller = CreateController();

        // Get any variable that exists
        var allResult = await controller.GetVariables();
        var allOk = allResult.Result as OkObjectResult;
        var variables = allOk?.Value as IEnumerable<VariableModel>;
        var firstVar = variables?.FirstOrDefault();

        if (firstVar is null)
        {
            // No variables registered, skip test
            return;
        }

        var result = await controller.GetVariable(firstVar.Name);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetResourceNames_ReturnsOkResult()
    {
        var controller = CreateController();

        var result = await controller.GetResourceNames();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetValues_ReturnsOkResult()
    {
        var controller = CreateController();

        var result = await controller.GetValues();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetValues_ReturnsDictionary()
    {
        var controller = CreateController();

        var result = await controller.GetValues();
        var ok = result.Result as OkObjectResult;

        Assert.NotNull(ok);
        Assert.IsAssignableFrom<IDictionary<string, object>>(ok.Value);
    }
}
