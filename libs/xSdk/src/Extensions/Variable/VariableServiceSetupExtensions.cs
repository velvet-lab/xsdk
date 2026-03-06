using xSdk.Extensions.Variable.Providers;

namespace xSdk.Extensions.Variable
{
    public static class VariableServiceSetupExtensions
    {
        public static VariableServiceSetup AddEnvironmentVariablesWithoutSetup(this VariableServiceSetup setup)
        {
            setup.AddEnvironmentVariablesWithoutSetup = true;
            return setup;
        }

        //public static VariableServiceSetup AddCommandlineVariablesWithoutSetup(this VariableServiceSetup setup)
        //{
        //    setup.AddCommanlineVariablesWithoutSetup = true;
        //    return setup;
        //}

        public static VariableServiceSetup RegisterProvider<TProvider>(this VariableServiceSetup setup)
            where TProvider : VariableProvider, new()
        {
            setup.Providers.Add(typeof(TProvider));

            return setup;
        }
    }
}
