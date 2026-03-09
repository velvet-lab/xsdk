using Swashbuckle.AspNetCore.Filters;

namespace xSdk.Data.Models;

internal class PingResponseExample : IExamplesProvider<string>
{
    public string GetExamples()
    {
        return "pong";
    }
}
