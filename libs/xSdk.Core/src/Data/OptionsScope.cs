using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Data;

[Flags]
public enum OptionsScope
{
    None = 0b_0000_0000,
    Default = 0b_0000_0001,
    Datalayer = 0b_0000_0010,
}
