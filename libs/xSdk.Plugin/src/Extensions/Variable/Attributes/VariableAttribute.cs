using xSdk.Data.Annotations;

namespace xSdk.Extensions.Variable.Attributes
{
    /// <summary>
    /// Default primaryKey attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class VariableAttribute : DataAnnotationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the DefaultAttribute class.
        /// </summary>
        /// <param name="value">The default primaryKey.</param>
        public VariableAttribute(
            string name = default,
            object defaultValue = default,
            string template = default,
            string prefix = default,
            string helpText = default,
            bool protect = false,
            bool hidden = false,
            string[] resourceNames = default,
            bool noPrefix = false
        )
            : base(defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
            Template = template;
            HelpText = helpText;
            Protect = protect;
            Prefix = prefix;
            Hidden = hidden;
            ResourceNames = resourceNames;
            NoPrefix = noPrefix;
        }

        public string Name { get; }

        public object DefaultValue { get; }

        public string Template { get; }

        public string Prefix { get; }

        public bool NoPrefix { get; }

        public string HelpText { get; }

        public bool Protect { get; }

        public bool CopySlimValue { get; }

        public bool Hidden { get; }

        public string[] ResourceNames { get; }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}
