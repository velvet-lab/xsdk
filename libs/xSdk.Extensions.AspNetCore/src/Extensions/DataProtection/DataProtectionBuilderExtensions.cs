using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace xSdk.Extensions.DataProtection;

public static class DataProtectionBuilderExtensions
{
    extension(DataProtectionBuilder builder)
    {
        public DataProtectionBuilder ConfigureDataProtection(Action<IDataProtectionBuilder> configure)
        {
            builder.ConfigureDataProtectionAction = configure;
            return builder;
        }
    }
}
