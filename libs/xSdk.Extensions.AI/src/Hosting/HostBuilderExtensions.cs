//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.ObjectPool;
//using xSdk.Data;
//using xSdk.Extensions.AI;

//namespace xSdk.Hosting;

//public static class HostBuilderExtensions
//{
//    extension(IHostBuilder hostBuilder)
//    {
//        public IHostBuilder AddAILayer(Action<IAILayerBuilder> factory)
//        {
//            hostBuilder
//                .ConfigureServices((_, services) =>
//                {
//                    var aiLayerBuilder = new AILayerBuilder(services);

//                    //services

//                    //    .AddSingleton<IDatalayerFactory>(provider =>
//                    //    {
//                    //        DatalayerFactory factoryInstance = ActivatorUtilities.CreateInstance<DatalayerFactory>(provider);
//                    //        return factoryInstance;
//                    //    })
//                    //    // Add pool provider for pooled database instances
//                    //    .TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

//                    factory?.Invoke(aiLayerBuilder);
//                });

//            return hostBuilder;
//        }
//    }
//}
