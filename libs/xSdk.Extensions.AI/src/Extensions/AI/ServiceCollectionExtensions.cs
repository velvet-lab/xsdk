using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using xSdk.Plugins.AI;

namespace xSdk.Extensions.AI;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAI(Action<AIBuilder> builder)
        {
            builder(AIBuilder.Instance);
            return services;
        }
    }
}
