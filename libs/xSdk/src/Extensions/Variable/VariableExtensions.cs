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

using xSdk.Extensions.Variable.Attributes;

namespace xSdk.Extensions.Variable;

internal static class VariableExtensions
{
    internal static IVariable SetPrefix(this IVariable variable, string prefix)
    {
        if (variable.TryCast(out Variable casted) && !string.IsNullOrEmpty(prefix))
        {
            casted.Prefix = prefix.Replace("setup", "", StringComparison.InvariantCultureIgnoreCase).Trim().ToLower();
        }

        return variable;
    }

    internal static IVariable SetAttribute(this IVariable variable, VariableAttribute attribute)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.Attribute = attribute;
        }

        return variable;
    }

    internal static IVariable SetTelemetryResourceValueDelegate(this IVariable variable, Func<object?> resourceDelegate)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.TelemetryResourceValue = resourceDelegate;
        }

        return variable;
    }

    internal static IVariable SetHelpText(this IVariable variable, string helpText)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.HelpText = helpText;
        }

        return variable;
    }

    public static IVariable Protect(this IVariable variable)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.IsProtected = true;
        }

        return variable;
    }

    internal static IVariable SetTemplate(this IVariable variable, string template)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.Template = template;
        }

        return variable;
    }

    internal static IVariable Hide(this IVariable variable)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.IsHidden = true;
        }

        return variable;
    }

    internal static IVariable DisablePrefix(this IVariable variable)
    {
        if (variable.TryCast(out Variable casted))
        {
            casted.NoPrefix = true;
        }

        return variable;
    }

    private static bool TryCast(this IVariable variable, out Variable result)
    {
#nullable disable
        result = default;
#nullable restore

        if (variable is Variable casted)
        {
            result = casted;
            return true;
        }

        return false;
    }
}
