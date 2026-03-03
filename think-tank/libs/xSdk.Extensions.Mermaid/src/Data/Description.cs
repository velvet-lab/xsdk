using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Extensions.Mermaid.Data
{
    internal sealed class Description : ISupportName, ISupportComments
    {
        public string Name { get; internal set; }

        public string Text { get; set; }

        public string Comment { get; set; }
    }
}
