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
using Microsoft.Extensions.Logging;
using xSdk.Data;

namespace xSdk.Extensions.Links;

internal sealed partial class LinksService(LinksOptions linksOptions, IHttpContextAccessor context, ILogger<LinksService> logger) : ILinksService
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1873:Potenziell kostspielige Protokollierung vermeiden", Justification = "<Ausstehend>")]
    public Task AddLinksAsync<TModel>(IEnumerable<TModel> model, CancellationToken cancellationToken = default)
        where TModel : class, IModel
    {
        logger.LogInformation("Add links to model collection of type {ModelType}", typeof(TModel).FullName);

        foreach (TModel item in model)
        {
            AddLinksInternal(item);
        }

        return Task.CompletedTask;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1873:Potenziell kostspielige Protokollierung vermeiden", Justification = "<Ausstehend>")]
    public Task AddLinksAsync<TModel>(TModel model, CancellationToken cancellationToken = default)
        where TModel : class, IModel
    {
        logger.LogInformation("Add links to model of type {ModelType}", typeof(TModel).FullName);
        AddLinksInternal(model);
        return Task.CompletedTask;
    }

    private void AddLinksInternal<TModel>(TModel model)
        where TModel : class, IModel
    {
        List<MethodDescription> descriptions = MethodAnalyzer.Analyze(context.HttpContext);
        var links = new Dictionary<string, IHateoasItem>();

        foreach (MethodDescription description in descriptions)
        {
            RoutedLink? link = SearchPolicyLink(model, description, context.HttpContext);
            if (link is RoutedLink routedLink && !links.ContainsKey(link.Name))
            {
                IHateoasItem? linkItem = routedLink.Build();
                if (linkItem != null)
                {
                    links.Add(routedLink.Name, linkItem);
                }
            }
        }

        SaveLinks(model, links);
    }

    private RoutedLink? SearchPolicyLink(IModel model, MethodDescription description, HttpContext? context)
    {
        foreach (IPolicy policy in linksOptions.Policies)
        {
            foreach (IRoutedLink link in policy.Links)
            {
                if (link is not RoutedLink linkInstance)
                {
                    continue;
                }

                if (string.Compare(link.MethodName, description.MethodName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    linkInstance.Model = model;
                    linkInstance.Description = description;
                    linkInstance.Context = context;
                    return linkInstance;
                }
            }
        }

        return default;
    }

    private static void SaveLinks(IModel model, IDictionary<string, IHateoasItem> links)
    {
        if (model is Model concreteModel)
        {
            var converted = links.ToDictionary(x => x.Key, x => x.Value);
            concreteModel.AdditionalData = new Dictionary<string, object>
            {
                { "_links", converted }
            };
        }
    }
}
