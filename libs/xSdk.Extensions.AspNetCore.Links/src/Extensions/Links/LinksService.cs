using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop.Implementation;
using xSdk.Data;

namespace xSdk.Extensions.Links;

internal sealed partial class LinksService(LinksOptions options, IHttpContextAccessor context, IServiceProvider provider, ILogger<LinksService> logger) : ILinksService
{
    public Task AddLinksAsync<TModel>(IEnumerable<TModel> model, CancellationToken cancellationToken = default)
        where TModel : class, IModel
    {
        foreach (var item in model)
        {
            AddLinksInternal(item);
        }

        return Task.CompletedTask;
    }

    public Task AddLinksAsync<TModel>(TModel model, CancellationToken cancellationToken = default)
        where TModel : class, IModel
    {
        AddLinksInternal(model);
        return Task.CompletedTask;
    }

    private void AddLinksInternal<TModel>(TModel model)
        where TModel : class, IModel
    {
        var descriptions = MethodAnalyzer.Analyze(context.HttpContext);
        var links = new Dictionary<string, IHateoasItem>();

        foreach (var description in descriptions)
        {
            var link = SearchPolicyLink(model, description, context.HttpContext);
            if (link != null && !links.ContainsKey(link.Name))
            {
                if (link is RoutedLink routedLink)
                {
                    var linkItem = routedLink.Build();
                    if (linkItem != null)
                    {
                        links.Add(link.Name, linkItem);
                    }
                }
            }
        }

        SaveLinks(model, links);
    }

    private RoutedLink? SearchPolicyLink(IModel model, MethodDescription description, HttpContext? context)
    {
        foreach (var policy in options.Policies)
        {
            foreach (var link in policy.Links)
            {
                if (string.Compare(link.MethodName, description.MethodName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    link.Model = model;
                    link.Description = description;
                    link.Context = context;
                    return link;
                }
            }
        }
        return default;
    }

    private void SaveLinks(IModel model, IDictionary<string, IHateoasItem> links)
    {
        if (model is Model concreteModel)
        {
            var converted = links.ToDictionary(x => x.Key, x => x.Value);
            concreteModel.AdditionalData = new Dictionary<string, object>();
            concreteModel.AdditionalData.Add("_links", converted);
        }
    }
}
