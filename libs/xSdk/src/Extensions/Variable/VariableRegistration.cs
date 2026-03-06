namespace xSdk.Extensions.Variable
{
    internal class VariableRegistration
    {
        internal virtual Type Type { get; }

        internal virtual ISetup Create(VariableService service)
        {
            return null;
        }
    }

    internal class VariableRegistration<TSetup> : VariableRegistration
        where TSetup : class, ISetup, new()
    {
        internal Action<TSetup> Configure { get; set; }

        internal TSetup Implementation { get; set; }

        internal override Type Type => typeof(TSetup);

        internal override ISetup Create(VariableService service)
        {
            if (Implementation == null)
            {
                Implementation = new TSetup();
            }

            if (Implementation is Setup abstractSetup)
            {
                abstractSetup.InitializeInternal(service);
            }
            Configure?.Invoke(Implementation);

            return Implementation;
        }
    }
}
