using xSdk.Shared;
using System.ComponentModel.DataAnnotations;

namespace xSdk.Data.Annotations
{
    public abstract class DataAnnotationAttribute : ValidationAttribute
    {
        private object _configuredValue;
        private object _currentValue;
        private Type _currentType;

        protected DataAnnotationAttribute(object value)
        {
            this._configuredValue = value;
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
}
