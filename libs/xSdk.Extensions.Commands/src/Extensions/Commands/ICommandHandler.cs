using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace xSdk.Extensions.Commands;

public interface ICommandHandler
{
    int Execute();

    Task<int> ExecuteAsync(CancellationToken cancellationToken);
}
