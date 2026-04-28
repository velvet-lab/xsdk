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
using xSdk.Shared;

namespace xSdk.Data.Annotations;

public abstract class DataAnnotationAttribute : ValidationAttribute
{
    private object _configuredValue;
    private object _currentValue;
    private Type _currentType;

    protected DataAnnotationAttribute(object value)
    {
        _configuredValue = value;
    }

    internal int GetIntValue() => (int)_configuredValue;

    internal double GetDoubleValue() => (double)_configuredValue;

    internal bool GetBoolValue() => (bool)_configuredValue;

    internal string GetStringValue() => (string)_configuredValue;

    internal object GetValue() => _configuredValue;

    internal TimeSpan GetTimeSpanValue() => TimeSpanParser.Parse(_configuredValue);

    internal object Value => _currentValue;

    internal Type Type => _currentType;

    internal bool IsIntValue() => _currentType == typeof(int);

    internal bool IsDoubleValue() => _currentType == typeof(double);

    internal bool IsBoolValue() => _currentType == typeof(bool);

    internal bool IsTimeSpanValue() => _currentType == typeof(TimeSpan);

    internal bool IsStringValue() => _currentType == typeof(string);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // TRICKY: This will only used to convert the correct Value to Destination Type
        var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
        if (property != null)
        {
            _currentValue = property.GetValue(validationContext.ObjectInstance);
            _currentType = property.PropertyType;

            if (_currentValue != null)
                _currentValue = Convert.ChangeType(_currentValue, _currentType);

            if (_configuredValue != null && _currentType != typeof(TimeSpan))
                _configuredValue = Convert.ChangeType(_configuredValue, _currentType);
        }

        return base.IsValid(value, validationContext);
    }
}
