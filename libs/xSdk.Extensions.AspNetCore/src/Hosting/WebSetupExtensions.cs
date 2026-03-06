//using NLog;

//namespace xSdk.Hosting
//{
//    public static class WebSetupExtensions
//    {
//        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

//        public static string BuildBaseUrl(this WebHostSetup setup) =>
//            BuildBaseUrl(setup.Bind, setup.Http, setup.Https, setup.IsHttpsEnabled);

//        public static string BuildApiUrl(this WebHostSetup setup, string apiVersion = "1") =>
//            setup.BuildApiUrl(null, apiVersion);

//        public static string BuildApiUrl(
//            this WebHostSetup setup,
//            string path,
//            string apiVersion = "1"
//        ) =>
//            BuildApiUrl(
//                setup.Bind,
//                setup.Http,
//                setup.Https,
//                setup.IsHttpsEnabled,
//                path,
//                apiVersion
//            );

//        private static string BuildApiUrl(
//            string bind,
//            int http,
//            int https,
//            bool isHttpsEnabled,
//            string path,
//            string apiVersion = "1"
//        )
//        {
//            var prefix = $"api/v{apiVersion}";

//            var baseUrl = BuildBaseUrl(bind, http, https, isHttpsEnabled);
//            var result = $"{baseUrl}/{prefix}";
//            if (!string.IsNullOrEmpty(path))
//                result = $"{result}/{path}";

//            logger.Info("Builded API Url '{0}'", result);
//            return result;
//        }

//        private static string BuildBaseUrl(string bind, int http, int https, bool isHttpsEnabled)
//        {
//            var scheme = "http://";
//            var baseurl = "";

//            var port = http;
//            if (isHttpsEnabled)
//            {
//                scheme = "https://";
//                port = https;
//            }

//            var portAsString = $":{port}";
//            if (port == 80 || port == 443)
//                portAsString = "";

//            baseurl = $"{scheme}{bind}{portAsString}";

//            logger.Trace("Builded Base Url '{0}'", baseurl);
//            return baseurl;
//        }
//    }
//}
