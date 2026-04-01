using xSdk.Hosting;

namespace xSdk.Extensions.Variable;

internal static class SetupLoader
{
    private static readonly Dictionary<Type, ISetup> _setupCache = new();

    internal static TSetup Load<TSetup>()
        where TSetup : class, ISetup
    {
        var setupType = typeof(TSetup);
        if (_setupCache.TryGetValue(setupType, out var cachedSetup))
        {
            return (TSetup)cachedSetup;
        }

        var setup = SlimHost.Instance.VariableSystem.GetSetup<TSetup>();
        if (setup != null)
        {
            setup.Validate(true);
            _setupCache[setupType] = setup;

            return setup;
        }

        throw new InvalidOperationException(string.Format("Setup of type {0} not found.", typeof(TSetup).FullName));
    }
}
