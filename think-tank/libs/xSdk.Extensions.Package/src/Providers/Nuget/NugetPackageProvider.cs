using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sewer56.Update.Interfaces;
using Sewer56.Update.Resolvers.NuGet;

namespace xSdk.Extensions.Package.Providers.Nuget
{
    internal class NugetPackageProvider(NugetSetup setup) : IPackageProvider
    {
        public IPackageResolver GetResolver()
        {
            throw new NotImplementedException();
        }
    }
}
