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

using System.Reflection;

namespace xSdk.Tools;

/// <summary>
/// <see cref="PropertyInfo"/> extension methods.
/// </summary>
public static class AttributeExtensions
{
    public static TAttribute? GetAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute
    {
        var attrs = type.GetCustomAttributes(typeof(TAttribute), inherit: false);
        if (attrs != null && attrs.Length > 0)
        {
            if (attrs.Length == 1)
                return attrs.Single() as TAttribute;
            else
                throw new SdkException($"More than one Attribute of Type '{typeof(TAttribute)}' is given");
        }

        return default;
    }

    /// <summary>
    /// Gets the specified <paramref name="propertyInfo"/> attribute of type <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type.</typeparam>
    /// <param name="propertyInfo">The <see cref="PropertyInfo" />.</param>
    /// <returns>The attribute, or <c>null</c>.</returns>
    public static TAttribute? GetAttribute<TAttribute>(this PropertyInfo propertyInfo)
        where TAttribute : Attribute
    {
        var attrs = propertyInfo.GetCustomAttributes(typeof(TAttribute), inherit: false);
        if (attrs != null && attrs.Length > 0)
        {
            if (attrs.Length == 1)
                return attrs.Single() as TAttribute;
            else
                throw new SdkException($"More than one Attribute of Type '{typeof(TAttribute)}' is given");
        }

        return default;
    }
}
