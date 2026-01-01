using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;

namespace xSdk.Data.Converters.Yaml
{
    public sealed class SemVerVersionConverter : IYamlTypeConverter
    {
        // Unfortunately the API does not provide those in the ReadYaml and WriteYaml
        // methods, so we are forced to set them after creation.
        public IValueSerializer ValueSerializer { get; set; }
        public IValueDeserializer ValueDeserializer { get; set; }

        public bool Accepts(Type type) => type == typeof(SemVer);

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            SemVer result = default;

            var version = ValueDeserializer.DeserializeValue(parser, typeof(string), new SerializerState(), ValueDeserializer);
            if (version != null)
            {
                result = new SemVer(version.ToString());
            }
            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new SdkException("Yaml Serializing is not implemented");
        }

        public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
