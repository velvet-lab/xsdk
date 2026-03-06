namespace xSdk.Extensions.Variable
{
    public interface IVariable
    {
        bool IsHidden { get; }

        bool IsProtected { get; }

        string Name { get; }

        string Prefix { get; }

        bool NoPrefix { get; }

        string Template { get; }

        Type ValueType { get; }

        string HelpText { get; }
    }
}
