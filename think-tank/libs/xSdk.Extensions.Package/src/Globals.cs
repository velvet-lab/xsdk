using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Extensions.Package
{
    internal class Globals
    {
        internal const string SecurityContext = "vpack";
        internal const string DefaultUserAgent = "artifactory-client-dotnet";
        internal const string ApiBase = "/api";
        internal const string DetailHeaderName = "X-Result-Detail";
        public const string DefaultRepository = "repo";
        public const string DefaultLocation = "/public";
        internal const int ArtifactoryStoreOrderNumber = 99999;
    }
}
