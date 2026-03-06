using System.Collections.Generic;

namespace xSdk.Extensions.Mermaid.Data
{
    internal sealed class Fork : ISupportName, ISupportComments
    {
        public string Name { get; internal set; }

        public string Comment { get; set; }
    }
}
