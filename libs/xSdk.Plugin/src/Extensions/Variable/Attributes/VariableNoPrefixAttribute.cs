namespace xSdk.Extensions.Variable.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class VariableNoPrefixAttribute : Attribute
    {
        public VariableNoPrefixAttribute() { }
    }
}
