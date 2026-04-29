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
using xSdk.Shared;
using xSdk.Tools;

namespace xSdk.Extensions.CloudEvents;


public static class CloudEventExtensions
{
    public static CloudEvent AddAttribute<TValue>(this CloudEvent cloudEvent, string name, CloudEventAttributeType type, TValue value)
    {
        var attr = CloudEventFactory.CreateAttribute(name, type);
        cloudEvent[attr] = value;

        return cloudEvent;
    }

    public static CloudEvent RemoveAttribute(this CloudEvent cloudEvent, string name)
    {
        foreach (var attribute in cloudEvent.GetPopulatedAttributes())
        {
            if (string.Compare(attribute.Key.Name, name, true) == 0)
                cloudEvent[attribute.Key] = null;
        }

        return cloudEvent;
    }

    public static TValue GetAttributeValue<TValue>(this CloudEvent cloudEvent, string name)
    {
        TValue result = default;

        var attributes = cloudEvent.GetPopulatedAttributes();
        var cleanedName = StringTools.RemoveSpecialChars(name);

        var kvp = attributes.SingleOrDefault(x => string.Compare(x.Key.Name, cleanedName, true) == 0);

        try
        {
            if (kvp.Value != null)
                result = TypeConverter.ConvertTo<TValue>(kvp.Value);
        }
        catch { }

        return result;
    }

    public static string GetScope(this CloudEvent cloudEvent)
    {
        var scope = cloudEvent.Source.OriginalString.Replace(CloudEventFactory.SourceBaseUrl, "");

        if (scope.StartsWith("/"))
            scope = scope.Substring(1);

        return scope;
    }

    public static void SetDataObject(this CloudEvent cloudEvent, object data)
    {
        if (data == null)
            return;

        var scope = cloudEvent.GetScope();
        var uri = CreateDataObjectSchemeUri(data.GetType(), scope);
        if (data is Exception ex)
        {
            uri = CreateDataObjectSchemeUri(ex.GetType(), scope);
            data = ex.Message;
        }

        cloudEvent.Data = data;
        cloudEvent.DataSchema = uri;

        return;
    }

    internal static CloudEvent EnrichAttributes(this CloudEvent cloudEvent, IEnumerable<CloudEventAttribute> attributes)
    {
        var extensions = CloudEventFactory.MergeAttributes(attributes);
        foreach (var extension in extensions)
        {
            if (CloudEventFactory.TryGetValueForAttribute(extension, out object value))
                cloudEvent[extension] = value;
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

    public static object GetDataObject(this CloudEvent cloudEvent)
    {
        var requestedDataType = cloudEvent.GetDataObjectType();
        return cloudEvent.GetDataObject(requestedDataType);
    }

    public static TValue GetDataObject<TValue>(this CloudEvent cloudEvent)
        where TValue : class
    {
        var requestedDataType = cloudEvent.GetDataObjectType();
        if (requestedDataType == null)
            requestedDataType = typeof(TValue);

        var result = cloudEvent.GetDataObject(requestedDataType);
        if (result != null)
            return result as TValue;

        return default;
    }

    public static Type GetDataObjectType(this CloudEvent cloudEvent) => cloudEvent.GetDataObjectType(null);

    private static Type GetDataObjectType(this CloudEvent cloudEvent, Type dataType)
    {
        Type result = default;
        if (cloudEvent.Data != null)
        {
            var scope = cloudEvent.GetScope();

            var dataSchemeUri = cloudEvent.DataSchema;
            if (dataSchemeUri == null && dataType != null)
                dataSchemeUri = CreateDataObjectSchemeUri(dataType, scope);

            if (dataSchemeUri != null)
                result = ParseDataObjectSchemeUrl(dataSchemeUri.OriginalString, scope);
        }
        return result;
    }

    private static object ConvertDataObject(JsonElement element, Type dataType)
    {
        object result = null;

        var jsonAsString = string.Empty;
        if (element.ValueKind == JsonValueKind.Object)
            jsonAsString = element.GetRawText();
        else if (element.ValueKind == JsonValueKind.String)
            jsonAsString = element.GetString();
        else
            throw new SdkException("CloudEvent has a Json Value as DataObject, because no Conversion exists");

        if (!string.IsNullOrEmpty(jsonAsString))
        {
            if (JsonTools.IsJson(jsonAsString))
                result = JsonSerializer.Deserialize(jsonAsString, dataType, JsonTools.GetSerializerOptions());
            else
                throw new SdkException("CloudEvent has a unknown Json Format as DataObject");
        }

        return result;
    }

    private static object GetDataObject(this CloudEvent cloudEvent, Type requestedDataType)
    {
        if (cloudEvent != null && cloudEvent.Data != null)
        {
            if (cloudEvent.Data is JsonElement json)
            {
                if (requestedDataType == null)
                    requestedDataType = typeof(object);

                return ConvertDataObject(json, requestedDataType);
            }
            else
                return cloudEvent.Data;
        }
        return null;
    }

    private static Uri CreateDataObjectSchemeUri(Type dataType, string scope)
    {
        var (sourceBaseUrl, schemeBaseUrl) = CloudEventFactory.CreateBaseUrls(scope);

        var assemblyName = dataType.Assembly.GetName().Name;

        return new Uri($"{schemeBaseUrl}/{assemblyName}?type={dataType.FullName}".ToLower(), UriKind.Absolute);
    }

    private static Type ParseDataObjectSchemeUrl(string dataSchemeUrl, string scope)
    {
        Type result = default;

        var (sourceBaseUrl, schemeBaseUrl) = CloudEventFactory.CreateBaseUrls(scope);
        var dataTypeUrl = dataSchemeUrl.Replace(schemeBaseUrl, "");

        if (!string.IsNullOrEmpty(dataTypeUrl))
        {
            var assemblyName = string.Empty;
            var typeName = string.Empty;

            if (dataTypeUrl.IndexOf("?") > -1)
            {
                var splitted = dataTypeUrl.Split("?", StringSplitOptions.RemoveEmptyEntries);
                assemblyName = splitted[0];

                if (splitted.Length > 1)
                {
                    splitted = splitted[1].Split("=", StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length > 1)
                        typeName = splitted[1];
                }
            }
            else
                assemblyName = dataTypeUrl;

            if (assemblyName.StartsWith("/"))
                assemblyName = assemblyName.Substring(1);

            if (!string.IsNullOrEmpty(typeName))
                result = Type.GetType($"{typeName.Trim()}, {assemblyName.Trim()}", false, true);
        }

        return result;
    }
}
