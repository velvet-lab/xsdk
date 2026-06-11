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

using System.Security.Claims;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xSdk.Data;
using xSdk.Extensions.Logging;
using xSdk.Security;

namespace xSdk.Extensions.Links;

internal static class RoutedLinkBuilder
{
    private static ILogger? _logger;
    private static ILogger Logger => _logger ??= LogManager.CreateLogger(typeof(RoutedLinkBuilder));

    internal static IHateoasItem? Build<TModel>(RoutedLink<TModel> link)
        where TModel : IModel
    {
        Logger.LogInformation("Build links");

        MethodDescription? description = link.Description;
        if (description != null)
        {
            Logger.LogDebug("Create base url path");
            string? baseUrl = CreateBaseUrl(link);

            if (!string.IsNullOrEmpty(baseUrl))
            {
                Logger.LogDebug("Replace values");
                string? href = ReplaceValue(link, baseUrl);

                if (!string.IsNullOrEmpty(href))
                {
                    bool isAuthorized = IsAuthorized(link);

                    if (isAuthorized)
                    {
                        Logger.LogDebug("Create HateOas Item");
                        var item = new HateoasItem
                        {
                            Rel = description.ControllerType.Name + "/" + description.MethodName,
                            Href = href,
                            Method = description.HttpMethod.ToString().ToUpperInvariant()
                        };
                        return item;
                    }
                }
            }
        }

        return default;
    }

    private static string? CleanControllerName(MethodDescription? description)
    {
        if (description != null)
        {
            Logger.LogDebug("Clean controller name");
            return description.ControllerType.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase);
        }

        return default;
    }

    private static string? CreateBaseUrl(RoutedLink link)
    {
        if (link.Description != null && link.Context != null)
        {
            string? controllerName = CleanControllerName(link.Description);

            if (!string.IsNullOrEmpty(controllerName))
            {
                HttpRequest request = link.Context.Request;
                string scheme = request.Scheme;
                string? host = request.Host.Value;

                string path = request.Path.Value ?? string.Empty;
                if (!string.IsNullOrEmpty(path) && path.IndexOf(controllerName, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    path = path.Substring(0, path.IndexOf(controllerName, StringComparison.OrdinalIgnoreCase));
                }

                string baseUrl = scheme + "://" + host;
                baseUrl = ConcatUrl(baseUrl, path, controllerName, link.Description.RouteTemplate ?? string.Empty);

                return baseUrl.ToLowerInvariant();
            }
        }

        return default;
    }

    private static string ConcatUrl(params string[] values)
    {
        string result = string.Empty;

        values
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList()
            .ForEach(item =>
            {
                if (item.StartsWith('/'))
                {
                    item = item.Substring(1);
                }

                if (!item.EndsWith('/'))
                {
                    result += item + "/";
                }
                else
                {
                    result += item;
                }
            });

        if (!string.IsNullOrEmpty(result) && result.EndsWith('/'))
        {
            result = result.Substring(0, result.Length - 1);
        }

        return result;
    }

    private static string? ReplaceValue<TModel>(RoutedLink<TModel> link, string baseUrl)
        where TModel : IModel
    {
        if (link.ConcreteModel is not null)
        {
            object? data = link.Values?.Invoke(link.ConcreteModel);

            string href = baseUrl.Replace("{", "{{").Replace("}", "}}");
            if (data != null)
            {
                HandlebarsTemplate<object, object> source = Handlebars.Compile(href);
                href = source(data);
            }

            return href;
        }

        return default;
    }

    private static bool IsAuthorized<TModel>(RoutedLink<TModel> link)
        where TModel : IModel
    {
        MethodDescription? description = link.Description;
        if (description != null && description.ShouldAuthorize)
        {
            HttpContext? context = link.Context;
            if (context != null)
            {
                ClaimsPrincipal user = context.User;

                if (!string.IsNullOrEmpty(description.AuthPolicy))
                {
                    IAuthorizationService? authService = context.RequestServices.GetService<IAuthorizationService>();
                    IAuthorizationPolicyProvider? policyProvider = context.RequestServices.GetService<IAuthorizationPolicyProvider>();

                    if (authService != null && policyProvider != null)
                    {
                        AuthorizationPolicy? policy = policyProvider.GetPolicyAsync(description.AuthPolicy).GetAwaiter().GetResult();
                        if (policy != null)
                        {
                            AuthorizationResult result = authService.AuthorizeAsync(user, policy).GetAwaiter().GetResult();
                            if (result.Succeeded)
                            {
                                return true;
                            }
                        }
                    }
                }

                if (description.AuthRoles.Length != 0)
                {
                    return description.AuthRoles.Any(user.IsInRole);
                }
            }

            return false;
        }

        return true;
    }
}
