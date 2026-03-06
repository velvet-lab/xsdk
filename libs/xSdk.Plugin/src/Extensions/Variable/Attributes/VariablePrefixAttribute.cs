namespace xSdk.Extensions.Variable.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class VariablePrefixAttribute : Attribute
    {
        public VariablePrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; }
    }
}
