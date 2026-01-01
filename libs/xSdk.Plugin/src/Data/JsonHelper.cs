using xSdk.Data.Converters.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Text.Json;

namespace xSdk.Data
{
    public static class JsonHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static bool IsJson(string data)
        {
            data = data.Trim();
            try
            {
                if (data.StartsWith("{") && data.EndsWith("}"))
                {
                    JToken.Parse(data);
                }
                else if (data.StartsWith("[") && data.EndsWith("]"))
                {
                    JArray.Parse(data);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Merge(string income, string outcome)
        {
            var result = new JObject();

            try
            {
                logger.Info("Try to merge States");

                if (string.IsNullOrEmpty(income))
                    income = "{}";

                if (string.IsNullOrEmpty(outcome))
                    outcome = "{}";

                if (!IsJson(income))
                    throw new SdkException("Parameter 'income' is not a valid Json");

                if (!IsJson(outcome))
                    throw new SdkException("Parameter 'outcome' is not a valid Json");

                var incomeAsJson = JObject.Parse(income);
                var outcomeAsJson = JObject.Parse(outcome);

                var mergeSettings = new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union };

                result.Merge(incomeAsJson, mergeSettings);
                result.Merge(outcomeAsJson, mergeSettings);
            }
            catch
            {
                logger.Warn("States could not merged.");
            }

            return result.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static System.Text.Json.JsonSerializerOptions GetSerializerOptions() => GetSerializerOptions(false);

        public static System.Text.Json.JsonSerializerOptions GetSerializerOptions(bool compact)
        {
            logger.Trace("Create new Json Serializer Options");

            var options = new System.Text.Json.JsonSerializerOptions();
            return ConfigureSerializerOptions(options, compact);
        }

        public static JsonSerializerOptions ConfigureSerializerOptions(this System.Text.Json.JsonSerializerOptions setup) =>
            ConfigureSerializerOptions(setup, false);

        public static JsonSerializerOptions ConfigureSerializerOptions(this System.Text.Json.JsonSerializerOptions setup, bool compact)
        {
            logger.Trace("Create new Json Serializer Options");

            setup.AllowTrailingCommas = true;
            setup.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            setup.IgnoreReadOnlyFields = false;
            setup.IgnoreReadOnlyProperties = false;
            setup.IncludeFields = false;
            // MaxDepth = 0,
            setup.PropertyNameCaseInsensitive = true;
            setup.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
            // ReferenceHandler = ReferenceHandler.Preserve,
            setup.WriteIndented = true;
            setup.Converters.Add(new DateTimeConverter());
            setup.Converters.Add(new SemVerConverter());
            setup.Converters.Add(new StageConverter());
            setup.Converters.Add(new VersionConverter());

            if (compact)
            {
                setup.WriteIndented = false;
            }

            return setup;
        }

        public static System.Text.Json.JsonDocumentOptions GetDocumentOptions()
        {
            var options = new System.Text.Json.JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = System.Text.Json.JsonCommentHandling.Skip };

            return options;
        }
    }
}
