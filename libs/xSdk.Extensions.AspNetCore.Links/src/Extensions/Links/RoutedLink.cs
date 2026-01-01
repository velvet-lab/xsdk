using xSdk.Data;
using Microsoft.AspNetCore.Http;

namespace xSdk.Extensions.Links
{
    public abstract class RoutedLink(string name, string methodName)
    {
        public string Name => name;

        public string MethodName => methodName;

        internal MethodDescription? Description { get; set; }

        internal HttpContext? Context { get; set; }

        internal IModel? Model { get; set; }

        internal abstract IHateoasItem? Build();
    }

    public class RoutedLink<TModel>(string name, string methodName, Func<TModel, object> values) : RoutedLink(name, methodName)
        where TModel : IModel
    {
        public Func<TModel, object> Values => values;

        internal TModel? ConcreteModel
        {
            get
            {
                if (Model is TModel concreteModel)
                {
                    return concreteModel;
                }
                return default;
            }
        }

        internal override IHateoasItem? Build()
        {
            var builder = new RoutedLinkBuilder();
            return builder.Build<TModel>(this);
        }
    }
}
