using CloudNative.CloudEvents;

namespace xSdk.Extensions.CloudEvents
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts a <see cref="CloudEvent"/> to requestes Exception
        /// </summary>
        /// <typeparam name="TException">A Exception to convert</typeparam>
        /// <param name="cloudEvent">The <see cref="CloudEvent"/></param>
        /// <returns>A Exception</returns>
        public static TException ToException<TException>(this CloudEvent cloudEvent)
            where TException : Exception
        {
            string errorMessage = default;

            // Return the Data from CloudEvent
            if (cloudEvent != null && cloudEvent.Data != null)
            {
                if (cloudEvent.Data is System.Text.Json.JsonElement json)
                {
                    if (json.ValueKind == System.Text.Json.JsonValueKind.String)
                        errorMessage = json.GetString();
                }
                else if (cloudEvent.Data is string)
                    errorMessage = cloudEvent.Data.ToString();
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                var exType = typeof(TException);
                var constructor = exType.GetConstructor(new Type[] { typeof(string) });
                if (constructor != null)
                    return constructor.Invoke(new object[] { errorMessage }) as TException;
            }

            return default;
        }

        /// <summary>
        /// Is given <see cref="CloudEvent"/> a Exception
        /// </summary>
        /// <param name="cloudEvent">The CloudEvent Object</param>
        /// <returns>True if CloudEvent is a <see cref="Exception"/>, otherwise false</returns>
        public static bool IsException(this CloudEvent cloudEvent)
        {
            var dataType = cloudEvent.GetDataObjectType();

            if (dataType != null)
            {
                if (typeof(Exception).IsAssignableFrom(dataType))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a <see cref="CloudEvent"/> Object from given Exception with a specific Scope/Category
        /// </summary>
        /// <param name="ex">The Exception which should converted to CloudEvent Object</param>
        /// <param name="scope">Scope for the Exception, e.g, aminoo/blueprint</param>
        /// <param name="type">Event Type for the Exception, e.g. blueprint.create.error</param>
        /// <param name="subject">A specific Subject to use. It could be tenant orientated Informations</param>
        /// <returns>A <see cref="CloudEvent"/></returns>
        public static CloudEvent ToCloudEvent(this Exception ex, string scope) => ex.ToCloudEvent(scope, null, null);

        /// <summary>
        /// Creates a <see cref="CloudEvent"/> Object from given Exception with a specific Scope/Category and Type
        /// </summary>
        /// <param name="ex">The Exception which should converted to CloudEvent Object</param>
        /// <param name="scope">Scope for the Exception, e.g, aminoo/blueprint</param>
        /// <param name="type">Event Type for the Exception, e.g. blueprint.create.error</param>
        /// <param name="subject">A specific Subject to use. It could be tenant orientated Informations</param>
        /// <returns>A <see cref="CloudEvent"/></returns>
        public static CloudEvent ToCloudEvent(this Exception ex, string scope, string type) => ex.ToCloudEvent(scope, type, null);

        /// <summary>
        /// Creates a <see cref="CloudEvent"/> Object from given Exception with a specific Scope/Category, Type and Subject
        /// </summary>
        /// <param name="ex">The Exception which should converted to CloudEvent Object</param>
        /// <param name="scope">Scope for the Exception, e.g, aminoo/blueprint</param>
        /// <param name="type">Event Type for the Exception, e.g. blueprint.create.error</param>
        /// <param name="subject">A specific Subject to use. It could be tenant orientated Informations</param>
        /// <returns>A <see cref="CloudEvent"/></returns>
        public static CloudEvent ToCloudEvent(this Exception ex, string scope, string type, string subject)
        {
            if (string.IsNullOrEmpty(type))
                type = "error";

            var (sourceBaseUrl, schemeBaseUrl) = CloudEventFactory.CreateBaseUrls(scope);
            var cloudEvent = CloudEventFactory.CreateRawCloudEvent(sourceBaseUrl, scope, type, subject, false, null);

            // The Data Object
            cloudEvent.SetDataObject(ex);

            return cloudEvent;
        }
    }
}
