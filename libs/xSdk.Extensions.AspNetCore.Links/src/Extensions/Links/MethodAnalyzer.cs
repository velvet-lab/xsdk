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

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace xSdk.Extensions.Links;

[ExcludeFromCodeCoverage(Justification = "Requires a live HttpContext with MVC endpoint metadata – integration-only.")]
internal static class MethodAnalyzer
{
    internal static List<MethodDescription> Analyze(HttpContext? context)
    {
        var descriptions = new List<MethodDescription>();

        TypeInfo? classType = SearchCallingController(context);
        if (classType != null)
        {
            MethodInfo[] methods = classType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                HttpMethodAttribute? httpAttribute = method.GetCustomAttribute<HttpMethodAttribute>();
                if (httpAttribute != null && httpAttribute.HttpMethods.Any() && !string.IsNullOrEmpty(httpAttribute.Name))
                {
                    var description = new MethodDescription
                    {
                        Action = method,
                        ControllerType = classType,
                        HttpMethod = HttpMethod.Parse(httpAttribute.HttpMethods.First()),
                        MethodName = httpAttribute.Name,
                        RouteTemplate = httpAttribute.Template
                    };

                    LinksAttribute? linksAttribute = method.GetCustomAttribute<LinksAttribute>();
                    if (linksAttribute != null)
                    {
                        description.PolicyName = linksAttribute.PolicyName;
                    }

                    AuthorizeAttribute? authorizeAttribute = method.GetCustomAttribute<AuthorizeAttribute>();
                    if (authorizeAttribute != null)
                    {
                        if (!string.IsNullOrEmpty(authorizeAttribute.Roles))
                        {
                            description.AuthRoles = authorizeAttribute.Roles.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        }

                        description.AuthPolicy = authorizeAttribute.Policy;
                    }

                    descriptions.Add(description);
                }
            }
        }

        return descriptions;
    }

    private static TypeInfo? SearchCallingController(HttpContext? context)
    {
        if (context != null)
        {
            Endpoint? endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                RequestDelegate? requestDelegate = endpoint.RequestDelegate;
                if (requestDelegate != null)
                {
                    object? target = requestDelegate.Target;
                    if (target != null)
                    {
                        FieldInfo? controllerProperty = target.GetType().GetField("controller");
                        if (controllerProperty != null && controllerProperty.GetValue(target) is ControllerActionDescriptor controller)
                        {
                            return controller.ControllerTypeInfo;
                        }
                    }
                }
            }
        }

        return default;
    }
}
