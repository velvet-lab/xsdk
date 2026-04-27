using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Plugins.Commands;

internal static class PromptFactory
{
    internal static Func<string> Factory { get; set; }
}
