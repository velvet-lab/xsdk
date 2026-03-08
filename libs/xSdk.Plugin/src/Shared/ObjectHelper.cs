namespace xSdk.Shared;

public static class ObjectHelper
{
    public static int CreateAutomaticHashCode(object obj)
    {
        var hash = 0;
        var objectType = obj.GetType();

        var properties = objectType.GetProperties();
        if (properties != null)
        {
            foreach (var property in properties)
            {
                CalcHash(property.GetValue(obj), ref hash);
            }
        }

        var fields = objectType.GetFields();
        if (fields != null)
        {
            foreach (var field in fields)
            {
                CalcHash(field.GetValue(obj), ref hash);
            }
        }

        return hash;
    }

    public static int CreateHashCode<TType>(TType value)
    {
        var hash = 0;
        CalcHash(value, ref hash);
        return hash;
    }

    public static bool Equals<TType>(object source, object dest, Func<TType, TType, bool> compare)
    {
        try
        {
            TType sourceCasted = (TType)source;
            TType destCasted = (TType)dest;
            return compare(sourceCasted, destCasted);
        }
        catch
        {
            return false;
        }
    }

    private static void CalcHash(object value, ref int hash)
    {
        const int index = 397;
        if (value != null)
        {
            unchecked
            {
                hash = hash * index ^ value.GetHashCode();
            }
        }
    }
}
