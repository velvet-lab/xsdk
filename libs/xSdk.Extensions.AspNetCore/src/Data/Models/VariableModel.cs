using xSdk.Extensions.Variable;
using Swashbuckle.AspNetCore.Annotations;

namespace xSdk.Data.Models
{
    [SwaggerSchema("Represents a variable")]
    public sealed class VariableModel
    {
        public VariableModel()
        {

        }

        internal VariableModel(IVariable variable)
        {
            Name = variable.Name;
            HelpText = variable.HelpText;
            Prefix = variable.Prefix;
            IsHidden = variable.IsHidden;
            IsProtected = variable.IsProtected;
            NoPrefix = variable.NoPrefix;
        }

        [SwaggerSchema("The name of the variable")]
        public string Name { get; set; }

        [SwaggerSchema("The help text for the variable")]
        public string HelpText { get; set; }

        [SwaggerSchema("Used prefix for the variable")]
        public string Prefix { get; set; }

        [SwaggerSchema("Is the variable hidden?")]
        public bool IsHidden { get; set; }

        [SwaggerSchema("Is the variable protected?")]
        public bool IsProtected { get; set; }

        [SwaggerSchema("Does the variable have a prefix?")]
        public bool NoPrefix { get; set; }
    }
}
