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

namespace xSdk.Controllers;

public class VariableControllerTests(TestHostFixture fixture) : IClassFixture<TestHostFixture>
{
    private VariableController CreateController()
    {
        IVariableService variableSvc = fixture.BuildHost().Services.GetRequiredService<IVariableService>();
        NullLogger<HealthController> logger = NullLogger<HealthController>.Instance;
        return new VariableController(variableSvc, logger);
    }

    [Fact]
    public async Task GetVariables_ReturnsOkResult()
    {
        VariableController controller = CreateController();

        ActionResult<IEnumerable<VariableModel>> result = await controller.GetVariables(TestContext.Current.CancellationToken);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetVariables_ReturnsListOfVariableModels()
    {
        VariableController controller = CreateController();

        ActionResult<IEnumerable<VariableModel>> result = await controller.GetVariables(TestContext.Current.CancellationToken);
        var ok = result.Result as OkObjectResult;

        Assert.NotNull(ok);
        Assert.IsType<IEnumerable<VariableModel>>(ok.Value);
    }

    [Fact]
    public async Task GetVariable_WithValidName_ReturnsOkResult()
    {
        VariableController controller = CreateController();

        // Get any variable that exists
        ActionResult<IEnumerable<VariableModel>> allResult = await controller.GetVariables(TestContext.Current.CancellationToken);
        var allOk = allResult.Result as OkObjectResult;
        var variables = allOk?.Value as IEnumerable<VariableModel>;
        VariableModel? firstVar = variables?.FirstOrDefault();

        if (firstVar is null)
        {
            // No variables registered, skip test
            return;
        }

        ActionResult<IEnumerable<VariableModel>> result = await controller.GetVariable(firstVar.Name, TestContext.Current.CancellationToken);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetResourceNames_ReturnsOkResult()
    {
        VariableController controller = CreateController();

        ActionResult<IDictionary<string, object>> result = await controller.GetResourceNames(TestContext.Current.CancellationToken);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetValues_ReturnsOkResult()
    {
        VariableController controller = CreateController();

        ActionResult<IDictionary<string, object>> result = await controller.GetValues(TestContext.Current.CancellationToken);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetValues_ReturnsDictionary()
    {
        VariableController controller = CreateController();

        ActionResult<IDictionary<string, object>> result = await controller.GetValues(TestContext.Current.CancellationToken);
        var ok = result.Result as OkObjectResult;

        Assert.NotNull(ok);
        Assert.IsType<IDictionary<string, object>>(ok.Value);
    }
}
