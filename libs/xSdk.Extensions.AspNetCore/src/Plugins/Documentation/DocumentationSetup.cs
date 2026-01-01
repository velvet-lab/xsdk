using xSdk.Extensions.Variable;
using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Plugins.Documentation
{
    [VariablePrefix("api_docu")]
    public sealed class DocumentationSetup : Setup, IDocumentationSetup
    {
        [Variable(
            name: Definitions.RoutePrefix.Name,
            template: Definitions.RoutePrefix.Template,
            helpText: Definitions.RoutePrefix.HelpText,
            defaultValue: Definitions.RoutePrefix.DefaultValue
        )]
        public string RoutePrefix
        {
            get => ReadValue<string>(Definitions.RoutePrefix.Name);
            set => SetValue(Definitions.RoutePrefix.Name, value);
        }

        //[Variable(
        //    name: Definitions.VariableDocumentation.Name,
        //    template: Definitions.VariableDocumentation.Template,
        //    helpText: Definitions.VariableDocumentation.HelpText,
        //    hidden: true,
        //    protect: true
        //)]
        //public bool ShowVariableDocumentation
        //{
        //    get => ReadValue<bool>(Definitions.VariableDocumentation.Name);
        //    set => SetValue(Definitions.VariableDocumentation.Name, value);
        //}

        public static class Definitions
        {
            public static class RoutePrefix
            {
                public const string Name = "route-prefix";
                public const string Template = "--route-prefix <route>";
                public const string HelpText = "RoutePrefix prefix for the api";
                public const string DefaultValue = "api/documentation";
            }

            public static class VariableDocumentation
            {
                public const string Name = "with-variable-documentation";
                public const string Template = "--with-variable-documentation";
                public const string HelpText = "Show documentation from Variable Controller";
            }
        }
    }
}
