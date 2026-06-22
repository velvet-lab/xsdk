using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Commands;

public interface IConsole
{
    Task<int> RunAsync(string[] args);
}
