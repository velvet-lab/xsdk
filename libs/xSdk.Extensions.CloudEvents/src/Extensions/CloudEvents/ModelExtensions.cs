using CloudNative.CloudEvents;
using xSdk.Data;

namespace xSdk.Extensions.CloudEvents
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Helper to converts a CloudEvent to requested Model
        /// </summary>
        /// <typeparam name="TModel">A Model Type <see cref="IModel"/></typeparam>
        /// <param name="cloudEvent">A <see cref="CloudEvent"/> Object</param>
        /// <returns>The Model</returns>
        public static TModel ToModel<TModel>(this CloudEvent cloudEvent)
            where TModel : class, IModel => cloudEvent.GetDataObject<TModel>();

        /// <summary>
        /// Creates a <see cref="CloudEvent"/> Object from given Model with a specific Type
        /// </summary>
        /// <typeparam name="TModel">A Model Type <see cref="IModel"/></typeparam>
        /// <param name="model">The Model which should converted to CloudEvent Object</param>
        /// <param name="type">Event Type for the Model, e.g. model.created</param>
        /// <returns>A <see cref="CloudEvent"/></returns>
        public static CloudEvent ToCloudEvent<TModel>(this TModel model, string type)
            where TModel : IModel => model.ToCloudEvent(null, type, null);

        /// <summary>
        /// Creates a <see cref="CloudEvent"/> Object from given Model with a specific Scope/Category and Type
        /// </summary>
        /// <typeparam name="TModel">A Model Type <see cref="IModel"/></typeparam>
        /// <param name="model">The Model which should converted to CloudEvent Object</param>
        /// <param name="scope">Scope for the Model, e.g, aminoo/blueprint</param>
        /// <param name="type">Event Type for the Model, e.g. model.created</param>
        /// <returns>A <see cref="CloudEvent"/></returns>
        public static CloudEvent ToCloudEvent<TModel>(this TModel model, string scope, string type)
            where TModel : IModel => model.ToCloudEvent(scope, type, null);

        /// <summary>
        /// Creates a <see cref="CloudEvent"/> Object from given Model with a specific Scope/Category, Type and Subject
        /// </summary>
        /// <typeparam name="TModel">A Model Type <see cref="IModel"/></typeparam>
        /// <param name="model">The Model which should converted to CloudEvent Object</param>
        /// <param name="scope">Scope for the Model, e.g, aminoo/blueprint</param>
        /// <param name="type">Event Type for the Model, e.g. model.created</param>
        /// <param name="subject">A specific Subject to use. It could be tenant orientated Informations</param>
        /// <returns>A <see cref="CloudEvent"/></returns>
        public static CloudEvent ToCloudEvent<TModel>(this TModel model, string scope, string type, string subject)
            where TModel : IModel
        {
            if (string.IsNullOrEmpty(scope))
                scope = model.GetType().Name;

            var (sourceBaseUrl, schemeBaseUrl) = CloudEventFactory.CreateBaseUrls(scope);
            var cloudEvent = CloudEventFactory.CreateRawCloudEvent(sourceBaseUrl, scope, type, subject, false, null);

            // The Data Object
            cloudEvent.SetDataObject(model);

            return cloudEvent;
        }
    }
}
