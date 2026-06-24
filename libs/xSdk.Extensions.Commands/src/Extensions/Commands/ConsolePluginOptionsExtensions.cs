using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Commands;

public static class ConsolePluginOptionsExtensions
{
    extension(ConsolePluginOptions options)
    {
        public ConsolePluginOptions DisableDefaultHelp()
        {
            options.DisableDefaultHelp = true;
            return options;
        }
    }
}
