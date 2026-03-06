using xSdk.Data;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace xSdk.Extensions.Links
{
    internal class RoutedLinkBuilder
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        internal IHateoasItem? Build<TModel>(RoutedLink<TModel> link)
            where TModel : IModel
        {
            logger.Info("Build links");

            var description = link.Description;
            if (description != null)
            {
                logger.Debug("Create base url path");
                var baseUrl = CreateBaseUrl(link);

                if (!string.IsNullOrEmpty(baseUrl))
                {
                    logger.Debug("Replace values");
                    var href = ReplaceValue(link, baseUrl);

                    if (!string.IsNullOrEmpty(href))
                    {
                        var isAuthorized = IsAuthorized(link);

                        if (isAuthorized)
                        {
                            logger.Debug("Create HateOas Item");
                            var item = new HateoasItem();
                            item.Rel = description.ControllerType.Name + "/" + description.MethodName;
                            item.Href = href;
                            item.Method = description.HttpMethod.ToString().ToUpperInvariant();
                            return item;
                        }
                    }
                }
            }

            return default;
        }

        private string? CleanControllerName(MethodDescription? description)
        {
            if (description != null)
            {
                logger.Debug("Clean controller name");
                return description.ControllerType.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase);
            }
            return default;
        }

        private string? CreateBaseUrl(RoutedLink link)
        {
            if (link.Description != null && link.Context != null)
            {
                var controllerName = CleanControllerName(link.Description);

                if (!string.IsNullOrEmpty(controllerName))
                {
                    var request = link.Context.Request;
                    var scheme = request.Scheme;
                    var host = request.Host.Value;

                    var path = request.Path.Value ?? string.Empty;
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (path.IndexOf(controllerName, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            path = path.Substring(0, path.IndexOf(controllerName, StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    var baseUrl = scheme + "://" + host;
                    baseUrl = ConcatUrl(baseUrl, path, controllerName, link.Description.RouteTemplate ?? string.Empty);

                    return baseUrl.ToLowerInvariant();
                }
            }
            return default;
        }

        private string ConcatUrl(params string[] values)
        {
            string result = string.Empty;
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var item = value;
                    if (item.StartsWith("/"))
                    {
                        item = item.Substring(1);
                    }

                    if (!item.EndsWith("/"))
                    {
                        result += item + "/";
                    }
                    else
                    {
                        result += item;
                    }
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                if (result.EndsWith("/"))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            return result;
        }

        private string? ReplaceValue<TModel>(RoutedLink<TModel> link, string baseUrl)
            where TModel : IModel
        {
            if (link.ConcreteModel != null)
            {
                var data = link.Values?.Invoke(link.ConcreteModel);

                var href = baseUrl.Replace("{", "{{").Replace("}", "}}");
                if (data != null)
                {
                    var source = Handlebars.Compile(href);
                    href = source(data);
                }

                return href;
            }
            return default;
        }

        private bool IsAuthorized<TModel>(RoutedLink<TModel> link)
            where TModel : IModel
        {
            var description = link.Description;
            if (description != null && description.ShouldAuthorize)
            {
                var context = link.Context;
                if (context != null)
                {
                    var user = context.User;

                    if (!string.IsNullOrEmpty(description.AuthPolicy))
                    {
                        var authService = context.RequestServices.GetService<IAuthorizationService>();
                        var policyProvider = context.RequestServices.GetService<IAuthorizationPolicyProvider>();

                        if (authService != null && policyProvider != null)
                        {
                            var policy = policyProvider.GetPolicyAsync(description.AuthPolicy).GetAwaiter().GetResult();
                            if (policy != null)
                            {
                                var result = authService.AuthorizeAsync(user, policy).GetAwaiter().GetResult();
                                if (result.Succeeded)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    if (description.AuthRoles.Any())
                    {
                        foreach (var role in description.AuthRoles)
                        {
                            if (user.IsInRole(role))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            return true;
        }
    }
}
