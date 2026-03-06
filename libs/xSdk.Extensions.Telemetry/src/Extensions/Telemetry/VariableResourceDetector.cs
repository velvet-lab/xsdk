using OpenTelemetry.Resources;

namespace xSdk.Extensions.Telemetry
{
    internal class VariableResourceDetector : IResourceDetector
    {
        private readonly IDictionary<string, object> _resources;

        internal VariableResourceDetector(IDictionary<string, object> resources)
        {
            this._resources = resources ?? throw new System.ArgumentNullException(nameof(resources));
        }

        public Resource Detect()
        {
            return new Resource(_resources);
        }
    }
}
