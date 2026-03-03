namespace xSdk.Extensions.Package
{
    internal class UpdateServiceBuilder(IServiceProvider provider) : IUpdateServiceBuilder
    {
        internal IList<Func<IServiceProvider, IPackageProvider>> registrations = new List<Func<IServiceProvider, IPackageProvider>>();

        internal void AddProvider(Func<IServiceProvider, IPackageProvider> action) => registrations.Add(action);

        public IUpdateService Build()
        {
            var packageProviders = new List<IPackageProvider>();
            foreach (var registration in registrations)
            {
                var packageProvider = registration(provider);
                if (packageProvider != null)
                {
                    packageProviders.Add(packageProvider);
                }
            }

            return new UpdateService(packageProviders);
        }
    }
}
