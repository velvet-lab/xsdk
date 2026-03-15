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

namespace xSdk.Extensions.Web;

[CLSCompliant(false)]
public static class ProblemDetailsExtensions
{
    public static ProblemDetails AsProblem(this Exception ex) => ex.AsProblem(StatusCodes.Status500InternalServerError, null);

    public static ProblemDetails AsProblem(this Exception ex, int status) => ex.AsProblem(status, null);

    public static ProblemDetails AsProblem(this Exception ex, int status, string? path)
    {
        var result = new ProblemDetails
        {
            Title = ex.Message,
            Detail = ex.StackTrace,
            Status = status,
        };

        if (ex.InnerException != null)
        {
            result.Title = ex.InnerException.Message;
        }

        if (!string.IsNullOrEmpty(path))
            result.Instance = path;

        return result;
    }

    public static BadRequestObjectResult BadRequestAsProblem(this ControllerBase controller, Exception ex)
    {
        var problem = ex.AsProblem(StatusCodes.Status400BadRequest, controller.HttpContext.Request.Path);
        return new BadRequestObjectResult(problem);
    }

    public static BadRequestObjectResult BadRequestAsProblem(this ControllerBase controller, string message) =>
        controller.BadRequestAsProblem(message, null);

    public static BadRequestObjectResult BadRequestAsProblem(this ControllerBase controller, string message, string? details)
    {
        var problem = new ProblemDetails
        {
            Title = message,
            Status = StatusCodes.Status400BadRequest,
            Instance = controller.HttpContext.Request.Path,
        };

        if (!string.IsNullOrEmpty(details))
            problem.Detail = details;

        return new BadRequestObjectResult(problem);
    }

    public static NotFoundObjectResult NotFoundAsProblem(this ControllerBase controller, string message) => controller.NotFoundAsProblem(message, null);

    public static NotFoundObjectResult NotFoundAsProblem(this ControllerBase controller, string message, string details)
    {
        var problem = new ProblemDetails
        {
            Title = message,
            Status = StatusCodes.Status404NotFound,
            Instance = controller.HttpContext.Request.Path,
        };

        if (!string.IsNullOrEmpty(details))
            problem.Detail = details;

        return new NotFoundObjectResult(problem);
    }
}
