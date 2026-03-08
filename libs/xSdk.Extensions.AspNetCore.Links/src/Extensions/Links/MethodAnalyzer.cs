using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace xSdk.Extensions.Links;

internal static class MethodAnalyzer
{
    internal static List<MethodDescription> Analyze(HttpContext? context)
    {
        var descriptions = new List<MethodDescription>();

        var classType = SearchCallingController(context);
        if (classType != null)
        {
            var methods = classType.GetMethods();
            foreach (var method in methods)
            {
                var httpAttribute = method.GetCustomAttribute<HttpMethodAttribute>();
                if (httpAttribute != null && httpAttribute.HttpMethods.Any() && !string.IsNullOrEmpty(httpAttribute.Name))
                {
                    var description = new MethodDescription();
                    description.Action = method;
                    description.ControllerType = classType;

                    description.HttpMethod = HttpMethod.Parse(httpAttribute.HttpMethods.First());
                    description.MethodName = httpAttribute.Name;
                    description.RouteTemplate = httpAttribute.Template;

                    var linksAttribute = method.GetCustomAttribute<LinksAttribute>();
                    if (linksAttribute != null)
                    {
                        description.PolicyName = linksAttribute.PolicyName;
                    }

                    var authorizeAttribute = method.GetCustomAttribute<AuthorizeAttribute>();
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
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var requestDelegate = endpoint.RequestDelegate;
                if (requestDelegate != null)
                {
                    var target = requestDelegate.Target;
                    if (target != null)
                    {
                        var controllerProperty = target.GetType().GetField("controller");
                        if (controllerProperty != null)
                        {
                            var controller = controllerProperty.GetValue(target) as ControllerActionDescriptor;
                            if (controller != null)
                            {
                                return controller.ControllerTypeInfo;
                            }
                        }
                    }
                }
            }
        }
        return default;
    }
}
