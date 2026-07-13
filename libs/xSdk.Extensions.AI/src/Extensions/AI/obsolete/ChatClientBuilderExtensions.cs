//using Microsoft.Extensions.AI;

//namespace xSdk.Extensions.AI;

//public static class ChatClientBuilderExtensions
//{
//    extension(Microsoft.Extensions.AI.ChatClientBuilder builder)
//    {
//        public Microsoft.Extensions.AI.ChatClientBuilder EnableTelemetry(bool enableSensitiveData)
//        {
//            builder
//                .UseOpenTelemetry(sourceName: Diagnostics.SourceName, configure: cfg => cfg.EnableSensitiveData = enableSensitiveData);
//            //.UseLogging();

//            return builder;
//        }
//    }
//}
