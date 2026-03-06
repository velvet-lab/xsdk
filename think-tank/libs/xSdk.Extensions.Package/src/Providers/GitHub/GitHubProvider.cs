using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sewer56.Update.Interfaces;
using Sewer56.Update.Resolvers.GitHub;

namespace xSdk.Extensions.Package.Providers.GitHub
{
    internal class GitHubProvider(GitHubSetup setup) : IPackageProvider
    {
        public IPackageResolver GetResolver()
        {
            return new GitHubReleaseResolver(new GitHubResolverConfiguration { RepositoryName = setup.Repository, UserName = setup.Owner });
        }
    }
}
