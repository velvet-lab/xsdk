using xSdk.Data;

namespace xSdk.Extensions.Links
{
    public sealed class Policy<TModel> : IPolicy
        where TModel : IModel
    {
        public List<RoutedLink> Links { get; } = new List<RoutedLink>();
    }
}
