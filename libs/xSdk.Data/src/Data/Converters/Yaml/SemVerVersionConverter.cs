/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;

namespace xSdk.Data.Converters.Yaml;

public sealed class SemVerVersionConverter : IYamlTypeConverter
{
    // Unfortunately the API does not provide those in the ReadYaml and WriteYaml
    // methods, so we are forced to set them after creation.
    public IValueSerializer ValueSerializer { get; set; }

    public IValueDeserializer ValueDeserializer { get; set; }

    public bool Accepts(Type type) => type == typeof(SemVer);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        SemVer? result = default;

        object? version = ValueDeserializer.DeserializeValue(parser, typeof(string), new SerializerState(), ValueDeserializer);
        if (version != null)
        {
            result = new SemVer((string)version);
        }

        return result;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type) => throw new SdkException("Yaml Serializing is not implemented");

    public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer) => throw new NotImplementedException();
}
