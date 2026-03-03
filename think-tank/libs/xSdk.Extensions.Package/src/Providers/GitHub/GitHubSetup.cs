using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Package.Providers.GitHub
{
    public sealed class GitHubSetup : Setup
    {
        public string Owner { get; set; }

        public string Repository { get; set; }
    }
}
