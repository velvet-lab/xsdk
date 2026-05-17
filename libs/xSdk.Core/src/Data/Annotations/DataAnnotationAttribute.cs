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

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using xSdk.Tools;

namespace xSdk.Data.Annotations;

public abstract class DataAnnotationAttribute(object value) : ValidationAttribute
{
    private object _configuredValue = value;

    internal int GetIntValue() => (int)_configuredValue;

    internal double GetDoubleValue() => (double)_configuredValue;

    internal bool GetBoolValue() => (bool)_configuredValue;

    internal string GetStringValue() => (string)_configuredValue;

    internal object GetValue() => _configuredValue;

    internal TimeSpan GetTimeSpanValue() => TimeSpanParser.Parse(_configuredValue);

    internal object? Value { get; private set; }

    internal Type? Type { get; private set; }

    internal bool IsIntValue() => Type == typeof(int);

    internal bool IsDoubleValue() => Type == typeof(double);

    internal bool IsBoolValue() => Type == typeof(bool);

    internal bool IsTimeSpanValue() => Type == typeof(TimeSpan);

    internal bool IsStringValue() => Type == typeof(string);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // TRICKY: This will only used to convert the correct Value to Destination Type
        string? memberName = validationContext.MemberName;
        if (string.IsNullOrEmpty(memberName))
        {
            return new ValidationResult("Validation failed.");
        }

        PropertyInfo? property = validationContext.ObjectType.GetProperty(memberName);
        if (property != null)
        {
            Value = property.GetValue(validationContext.ObjectInstance);
            Type = property.PropertyType;

            if (Value != null)
            {
                Value = Convert.ChangeType(Value, Type);
            }

            if (_configuredValue != null && Type != typeof(TimeSpan))
            {
                _configuredValue = Convert.ChangeType(_configuredValue, Type);
            }
        }

        return base.IsValid(value, validationContext);
    }
}
