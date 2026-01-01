namespace xSdk.Hosting
{
    public sealed class SlimHost
    {
        public static ISlimHost Instance => SlimHostInternal.Instance ?? throw new InvalidOperationException("SlimHost is not initialized");
    }
}
