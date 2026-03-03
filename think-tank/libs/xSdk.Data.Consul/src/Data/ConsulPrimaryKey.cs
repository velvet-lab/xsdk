using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Data
{
    public class ConsulPrimaryKey : PrimaryKey<string>
    {
        public ConsulPrimaryKey()
            : base(string.Empty) { }

        public ConsulPrimaryKey(object? initialValue)
            : base(initialValue) { }
    }
}
