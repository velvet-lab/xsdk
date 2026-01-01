using Newtonsoft.Json.Linq;

namespace xSdk.Shared
{
    public static class TypeConverter
    {
        private static readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(ushort),
            typeof(uint),
            typeof(float),
        };

        public static TValue ConvertTo<TValue>(object value)
        {
            TValue result = default;

            try
            {
                var tmp = ConvertTo(value, typeof(TValue));
                if (tmp != null)
                    // Try cast
                    result = (TValue)tmp;
                else if (value != null)
                    // Try direct cast
                    result = (TValue)value;
            }
            catch
            {
                try
                {
                    // Try direct Cast
                    if (value != null)
                        result = (TValue)value;
                }
                catch
                {
                    // nothing to tell
                }
            }

            return result;
        }

        public static object ConvertTo(object value, Type targetType)
        {
            object result = default;

            try
            {
                result = Convert.ChangeType(value, targetType);
            }
            catch
            {
                // nothing to tell
            }

            if ((result == null || IsEmpty(result, targetType)) && !IsEmpty(value, targetType))
            {
                try
                {
                    if (targetType == typeof(int))
                        result = Convert.ToInt32(value);
                    else if (targetType == typeof(short))
                        result = Convert.ToInt16(value);
                    else if (targetType == typeof(long))
                        result = Convert.ToInt64(value);
                    else if (targetType == typeof(double))
                        result = Convert.ToDouble(value);
                    else if (targetType == typeof(float))
                        result = Convert.ToSingle(value);
                    else if (targetType == typeof(bool))
                        result = Convert.ToBoolean(value);
                    else if (targetType == typeof(char))
                        result = Convert.ToChar(value);
                    else if (targetType == typeof(string))
                        result = Convert.ToString(value);
                    else if (targetType == typeof(byte))
                        result = Convert.ToByte(value);
                    else if (targetType == typeof(decimal))
                        result = Convert.ToDecimal(value);
                    else if (targetType == typeof(sbyte))
                        result = Convert.ToSByte(value);
                    else if (targetType == typeof(DateTime))
                        result = Convert.ToDateTime(value);
                    else if (targetType == typeof(TimeSpan))
                        result = TimeSpanParser.Parse(value);
                    else if (targetType == typeof(Guid))
                        result = Guid.Parse(value.ToString());
                    else if (targetType.IsEnum)
                        result = Enum.Parse(targetType, value.ToString());
                }
                catch
                {
                    // Nothing to tell
                }
            }

            return result;
        }

        public static Type GetValueType(object value)
        {
            if (value == null)
                return typeof(object);

            try
            {
                var token = JToken.Parse($"{value}");
                if (token.Type == JTokenType.Integer)
                    return typeof(int);
                else if (token.Type == JTokenType.Float)
                    return typeof(float);
                else if (token.Type == JTokenType.Boolean)
                    return typeof(bool);
            }
            catch
            {
                // nothing to tell
            }

            try
            {
                string? valueAsString = value.ToString();

                if (valueAsString?.IndexOf(@"\") > -1)
                {
                    valueAsString = valueAsString.Replace(@"\", "/");
                }

                var token = JToken.Parse($"'{valueAsString}'");
                if (token.Type == JTokenType.String)
                    return typeof(string);
                else if (token.Type == JTokenType.Date)
                    return typeof(DateTime);
                else if (token.Type == JTokenType.Guid)
                    return typeof(Guid);
                else if (token.Type == JTokenType.Uri)
                    return typeof(Uri);
                else if (token.Type == JTokenType.TimeSpan)
                    return typeof(TimeSpan);
            }
            catch
            {
                // nothing to tell
            }

            return default;
        }

        public static bool IsEmpty(object value, Type targetType)
        {
            try
            {
                if (targetType == typeof(Guid))
                {
                    if (Guid.TryParse(value.ToString(), out Guid tmp))
                        return tmp == Guid.Empty;
                }
                else if (targetType == typeof(TimeSpan))
                {
                    if (value is string tmpValue1)
                        return string.IsNullOrEmpty(tmpValue1);
                    else if (value is TimeSpan tmpValue2)
                        return tmpValue2 == TimeSpan.Zero;
                }
                else if (IsNumeric(targetType))
                {
                    if (value is int tmpValue1)
                        return tmpValue1 == 0;
                    else if (value is double tmpValue2)
                        return tmpValue2 == 0.0;
                    else if (value is decimal tmpValue3)
                        return tmpValue3 == 0;
                    else if (value is long tmpValue4)
                        return tmpValue4 == 0;
                    else if (value is short tmpValue5)
                        return tmpValue5 == 0;
                    else if (value is sbyte tmpValue6)
                        return tmpValue6 == 0;
                    else if (value is byte tmpValue7)
                        return tmpValue7 == 0;
                    else if (value is ulong tmpValue8)
                        return tmpValue8 == 0;
                    else if (value is ushort tmpValue9)
                        return tmpValue9 == 0;
                    else if (value is uint tmpValue10)
                        return tmpValue10 == 0;
                    else if (value is float tmpValue11)
                        return tmpValue11 == 0.0;
                }
                else if (targetType == typeof(string))
                {
                    if (value != null)
                        return string.IsNullOrEmpty(value.ToString());

                    return true;
                }
            }
            catch
            {
                // Nothing to tell
            }
            return false;
        }

        public static bool IsNumeric<TType>() => IsNumeric(typeof(TType));

        public static bool IsNumeric(Type myType)
        {
            return _numericTypes.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
        }
    }
}
