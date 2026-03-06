using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace xSdk.Data
{
    public sealed class ReleaseInfo
    {
        public ReleaseInfo()
        {
            Releases = new List<Release>();
        }

        //[JsonIgnore]
        //public string MetaFile { get; internal set; }

        //[JsonIgnore]
        //public string File { get; internal set; }

        //[JsonIgnore]
        //public string Source { get; internal set; }

        //[JsonIgnore]
        //public PackageModel Package { get; internal set; }

        public ICollection<Release> Releases { get; set; }

        public object ExtraData { get; set; }
    }
}
