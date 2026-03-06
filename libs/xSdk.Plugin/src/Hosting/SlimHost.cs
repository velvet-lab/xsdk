namespace xSdk.Hosting
{
    // Remarks: This SlimHost is only for Abstractions Library and should not be used in any other library.
    // The Real SlimHost is implemented in the xSdk library and will constructed with the Builder Pattern
    internal static class SlimHost
    {
        private static ISlimHost _slimHost;

        internal static void Configure(ISlimHost host)
        {
            _slimHost = host;
        }

        internal static ISlimHost Instance => _slimHost ?? throw new InvalidOperationException("SlimHost has not been initialized.");
    }
}
