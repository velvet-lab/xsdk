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
using xSdk.Extensions.Variable.Attributes;
using xSdk.Shared;

namespace xSdk.Extensions.Variable;

public static class VariableServiceExtensions
{
    public static void ParseForVariables(this IVariableService variableService, object implementation)
    {
        // Remarks: Dont activate Logging, because its produces a StackOverFlow
        MethodInfo? createMethod = typeof(Variable).GetMethod("Create", BindingFlags.Static | BindingFlags.Public, [typeof(string)]);
        if (createMethod != null)
        {
            // Read all properties of the Implementation
            Type setupType = implementation.GetType();

            string? mainPrefix = setupType
                .Name
                .Replace("implementation", "", StringComparison.InvariantCultureIgnoreCase)
                .Replace("impl", "", StringComparison.InvariantCultureIgnoreCase)
                .Replace("options", "", StringComparison.InvariantCultureIgnoreCase)
                .Replace("setup", "", StringComparison.InvariantCultureIgnoreCase);

            VariablePrefixAttribute? prefixAttribute = setupType.GetAttribute<VariablePrefixAttribute>();
            VariableNoPrefixAttribute? noprefixAttribute = setupType.GetAttribute<VariableNoPrefixAttribute>();
            if (prefixAttribute != null)
            {
                mainPrefix = prefixAttribute.Prefix;
            }

            if (noprefixAttribute != null)
            {
                mainPrefix = null;
            }

            foreach (var property in setupType.GetProperties())
            {
                VariableAttribute? attr = property.GetAttribute<VariableAttribute>();
                if (attr != null)
                {
                    object defaultValue = attr.DefaultValue;
                    object? currentValue = null;
                    try
                    {
                        currentValue = property.GetValue(implementation);
                    }
                    catch
                    {
                        // Nothing to tell
                    }

                    if (IsDefaultValueGreater(property.PropertyType, currentValue, defaultValue))
                    {
                        currentValue = null;
                    }

                    // Set Default Value
                    if (currentValue == null && defaultValue != null)
                    {
                        if (!TypeConverter.IsEmpty(defaultValue, property.PropertyType))
                        {
                            defaultValue = TypeConverter.ConvertTo(defaultValue, property.PropertyType);
                        }
                    }

                    // Create Variable
                    MethodInfo genericCreateMethod = createMethod.MakeGenericMethod(property.PropertyType);
                    if (genericCreateMethod != null)
                    {
                        string name = property.Name;
                        if (!string.IsNullOrEmpty(attr.Name))
                        {
                            name = attr.Name;
                        }

                        Variable? variable = genericCreateMethod.Invoke(null, [name.ToLower()]) as Variable;
                        if (variable != null)
                        {
                            if (!string.IsNullOrEmpty(mainPrefix))
                            {
                                variable.SetPrefix(mainPrefix);
                            }

                            if (!string.IsNullOrEmpty(attr.Prefix))
                            {
                                variable.SetPrefix(attr.Prefix);
                            }

                            if (defaultValue != null)
                            {
                                variable.SetDefaultValue(defaultValue);
                            }

                            if (!string.IsNullOrEmpty(attr.HelpText))
                            {
                                variable.SetHelpText(attr.HelpText);
                            }

                            if (!string.IsNullOrEmpty(attr.Template))
                            {
                                variable.SetTemplate(attr.Template);
                            }

                            if (attr.Protect)
                            {
                                variable.Protect();
                            }

                            if (attr.Hidden)
                            {
                                variable.Hide();
                            }

                            if (attr.NoPrefix)
                            {
                                variable.DisablePrefix();
                            }

                            variable.SetAttribute(attr);
                            variable.SetTelemetryResourceValueDelegate(() => property.GetValue(implementation));

                            ((VariableService)variableService).AddVariableFromSetupInitialize(variable);

                            // Try to read Environment for the correct Value
                            MethodInfo? readMethod = typeof(VariableService).GetMethod(
                                "ReadVariableValueInternal",
                                BindingFlags.NonPublic | BindingFlags.Instance,
                                [typeof(string), typeof(bool), typeof(bool)]
                            );
                            if (readMethod != null)
                            {
                                MethodInfo genericReadMethod = readMethod.MakeGenericMethod(property.PropertyType);
                                if (genericReadMethod != null)
                                {
                                    object? environmentValue = genericReadMethod.Invoke(variableService, [name, false, false]);
                                    if (environmentValue != null && !TypeConverter.IsEmpty(environmentValue, property.PropertyType))
                                    {
                                        if (!variable.IsProtected)
                                        {
                                            // Set the readed Value
                                            property.SetValue(implementation, environmentValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static bool IsDefaultValueGreater(Type type, object? currentValue, object defaultValue)
    {
        if (currentValue != null && defaultValue != null && type.IsEnum)
        {
            string? noneEnumValue = Enum.GetName(type, 0b_0000_0000);
            if (Enum.TryParse(type, currentValue.ToString(), out object? currentEnum))
            {
                string? currentEnumValue = Enum.GetName(type, currentEnum);
                if (currentEnumValue == noneEnumValue)
                {
                    if (Enum.TryParse(type, defaultValue.ToString(), out object? defaultEnum))
                    {
                        string? defaultEnumValue = Enum.GetName(type, defaultEnum);
                        if (defaultEnumValue != noneEnumValue)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
