namespace xSdk.Extensions.Commands.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class CommandArgumentAttribute : Attribute
{
    public string Name { get; }

    public CommandArgumentAttribute(string name)
    {
        Name = name;
    }
}
