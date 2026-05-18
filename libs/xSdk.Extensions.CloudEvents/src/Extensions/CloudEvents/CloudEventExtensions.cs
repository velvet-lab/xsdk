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

using System.Text.Json;
using CloudNative.CloudEvents;
using xSdk.Tools;

namespace xSdk.Extensions.CloudEvents;

public static class CloudEventExtensions
{
    public static CloudEvent AddAttribute<TValue>(this CloudEvent cloudEvent, string name, CloudEventAttributeType type, TValue value)
    {
        CloudEventAttribute attr = CloudEventFactory.CreateAttribute(name, type);
        cloudEvent[attr] = value;

        return cloudEvent;
    }

    public static CloudEvent RemoveAttribute(this CloudEvent cloudEvent, string name)
    {
        cloudEvent
            .GetPopulatedAttributes()
            .Where(x => string.Compare(x.Key.Name, name, true) == 0)
            .ToList()
            .ForEach(x => cloudEvent[x.Key] = null);

        return cloudEvent;
    }

    public static TValue? GetAttributeValue<TValue>(this CloudEvent cloudEvent, string name)
    {
        TValue? result = default;

        IEnumerable<KeyValuePair<CloudEventAttribute, object>> attributes = cloudEvent.GetPopulatedAttributes();
        string cleanedName = StringTools.RemoveSpecialChars(name);

        KeyValuePair<CloudEventAttribute, object> kvp = attributes.SingleOrDefault(x => string.Compare(x.Key.Name, cleanedName, true) == 0);

        try
        {
            if (kvp.Value != null)
            {
                result = TypeConverter.ConvertTo<TValue>(kvp.Value);
            }
        }
        catch
        {
            // Ignore conversion errors and return default value
        }

        return result;
    }

    public static string GetScope(this CloudEvent cloudEvent)
    {
        if (cloudEvent.Source == null)
        {
            throw new SdkException("CloudEvent has no Source defined, so Scope cannot be determined");
        }

        string scope = cloudEvent.Source.OriginalString.Replace(CloudEventFactory.SourceBaseUrl, "");
        if (scope.StartsWith('/'))
        {
            scope = scope.Substring(1);
        }

        return scope;
    }

    public static void SetDataObject(this CloudEvent cloudEvent, object data)
    {
        if (data == null)
        {
            return;
        }

        string scope = cloudEvent.GetScope();
        Uri uri = CreateDataObjectSchemeUri(data.GetType(), scope);
        if (data is Exception ex)
        {
            uri = CreateDataObjectSchemeUri(ex.GetType(), scope);
            data = ex.Message;
        }

        cloudEvent.Data = data;
        cloudEvent.DataSchema = uri;
    }

    internal static CloudEvent EnrichAttributes(this CloudEvent cloudEvent, IEnumerable<CloudEventAttribute>? attributes)
    {
        IEnumerable<CloudEventAttribute> extensions = CloudEventFactory.MergeAttributes(attributes);
        foreach (CloudEventAttribute extension in extensions)
        {
            if (CloudEventFactory.TryGetValueForAttribute(extension, out object? value) && value != null)
            {
                cloudEvent[extension] = value;
            }
        }

        return cloudEvent;
    }

    //public static TValue GetDataObjectValue<TValue>(this CloudEvent cloudEvent)
    //{
    //    if (cloudEvent != null && cloudEvent.Data != null)
    //    {
    //        if (cloudEvent.Data is System.Text.Json.JsonElement json)
    //        {
    //            var result = cloudEvent.GetDataObject();
    //            if (result is TValue value)
    //                return value;
    //            else
    //                cloudEvent.GetDataObject()
    //        }
    //        else if (cloudEvent.Data is TValue value)
    //            return value;
    //    }
    //    return default;
    //}

    public static object? GetDataObject(this CloudEvent cloudEvent)
    {
        Type? requestedDataType = cloudEvent.GetDataObjectType();
        if (requestedDataType != null)
        {
            return cloudEvent.GetDataObject(requestedDataType);
        }

        return default;
    }

    public static TValue? GetDataObject<TValue>(this CloudEvent cloudEvent)
        where TValue : class
    {
        Type requestedDataType = cloudEvent.GetDataObjectType() ?? typeof(TValue);
        object? result = cloudEvent.GetDataObject(requestedDataType);
        if (result != null)
        {
            return result as TValue;
        }

        return default;
    }

    public static Type? GetDataObjectType(this CloudEvent cloudEvent) => cloudEvent.GetDataObjectType(null);

    private static Type? GetDataObjectType(this CloudEvent cloudEvent, Type? dataType)
    {
        Type? result = default;
        if (cloudEvent.Data != null)
        {
            string scope = cloudEvent.GetScope();

            Uri? dataSchemeUri = cloudEvent.DataSchema;
            if (dataSchemeUri == null && dataType != null)
            {
                dataSchemeUri = CreateDataObjectSchemeUri(dataType, scope);
            }

            if (dataSchemeUri != null)
            {
                result = ParseDataObjectSchemeUrl(dataSchemeUri.OriginalString, scope);
            }
        }

        return result;
    }

    private static object? ConvertDataObject(JsonElement element, Type dataType)
    {
        object? result = null;

        string? jsonAsString;
        if (element.ValueKind == JsonValueKind.Object)
        {
            jsonAsString = element.GetRawText();
        }
        else if (element.ValueKind == JsonValueKind.String)
        {
            jsonAsString = element.GetString();
        }
        else
        {
            throw new SdkException("CloudEvent has a Json Value as DataObject, because no Conversion exists");
        }

        if (!string.IsNullOrEmpty(jsonAsString))
        {
            if (JsonTools.IsJson(jsonAsString))
            {
                result = JsonSerializer.Deserialize(jsonAsString, dataType, JsonTools.GetSerializerOptions());
            }
            else
            {
                throw new SdkException("CloudEvent has a unknown Json Format as DataObject");
            }
        }

        return result;
    }

    private static object? GetDataObject(this CloudEvent cloudEvent, Type requestedDataType)
    {
        if (cloudEvent != null && cloudEvent.Data != null)
        {
            if (cloudEvent.Data is JsonElement json)
            {
                if (requestedDataType == null)
                {
                    requestedDataType = typeof(object);
                }

                return ConvertDataObject(json, requestedDataType);
            }
            else
            {
                return cloudEvent.Data;
            }
        }

        return default;
    }

    private static Uri CreateDataObjectSchemeUri(Type dataType, string scope)
    {
        (string? _, string? schemeBaseUrl) = CloudEventFactory.CreateBaseUrls(scope);

        string? assemblyName = dataType.Assembly.GetName().Name;

        return new Uri($"{schemeBaseUrl}/{assemblyName}?type={dataType.FullName}".ToLower(), UriKind.Absolute);
    }

    private static Type? ParseDataObjectSchemeUrl(string dataSchemeUrl, string scope)
    {
        Type? result = default;

        (_, string? schemeBaseUrl) = CloudEventFactory.CreateBaseUrls(scope);
        string dataTypeUrl = dataSchemeUrl.Replace(schemeBaseUrl, "");

        if (!string.IsNullOrEmpty(dataTypeUrl))
        {
            string typeName = string.Empty;

            string assemblyName;
            if (dataTypeUrl.IndexOf('?') > -1)
            {
                string[] splitted = dataTypeUrl.Split('?', StringSplitOptions.RemoveEmptyEntries);
                assemblyName = splitted[0];

                if (splitted.Length > 1)
                {
                    splitted = splitted[1].Split('=', StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length > 1)
                    {
                        typeName = splitted[1];
                    }
                }
            }
            else
            {
                assemblyName = dataTypeUrl;
            }

            if (assemblyName.StartsWith('/'))
            {
                assemblyName = assemblyName.Substring(1);
            }

            if (!string.IsNullOrEmpty(typeName))
            {
                result = Type.GetType($"{typeName.Trim()}, {assemblyName.Trim()}", false, true);
            }
        }

        return result;
    }
}
