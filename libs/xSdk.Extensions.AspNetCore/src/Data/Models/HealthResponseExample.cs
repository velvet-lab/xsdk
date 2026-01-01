using Swashbuckle.AspNetCore.Filters;

namespace xSdk.Data.Models
{
    internal class HealthResponseExample : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Ok";
        }
    }
}
