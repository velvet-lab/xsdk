using Microsoft.AspNetCore.Hosting;
using xSdk.Extensions.Options;

namespace xSdk.Hosting;

public static class WebHostBuilderContextExtensions
{
    extension(WebHostBuilderContext context)
    {
        public void EnrichEnvironment(EnvironmentOptions options)
        {            
            if (context.HostingEnvironment.EnvironmentName != options.StageAsString)
            {
                context.HostingEnvironment.EnvironmentName = options.StageAsString;
            }
        }
    }
}
