using xSdk.Data;

namespace xSdk.Extensions.Links
{
    public static class PolicyExtensions
    {
        public static Policy<TModel> RequireRoutedLink<TModel>(this Policy<TModel> policy, string name, string route)
            where TModel : IModel
            => policy.RequireRoutedLink(name, route, null);


        public static Policy<TModel> RequireRoutedLink<TModel>(this Policy<TModel> policy, string name, string route, Func<TModel, object>? values)
            where TModel : IModel
        {
            var link = new RoutedLink<TModel>(name, route, values);
            policy.Links.Add(link);

            return policy;
        }
    }
}
