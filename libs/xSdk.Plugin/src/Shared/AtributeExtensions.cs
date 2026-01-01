using System.Reflection;

namespace xSdk.Shared
{
    /// <summary>
    /// <see cref="PropertyInfo"/> extension methods.
    /// </summary>
    public static class AtributeExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            var attrs = type.GetCustomAttributes(typeof(TAttribute), inherit: false);
            if (attrs != null && attrs.Length > 0)
            {
                if (attrs.Length == 1)
                    return attrs.SingleOrDefault() as TAttribute;
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
        public static TAttribute GetAttribute<TAttribute>(this PropertyInfo propertyInfo)
            where TAttribute : Attribute
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(TAttribute), inherit: false);
            if (attrs != null && attrs.Length > 0)
            {
                if (attrs.Length == 1)
                    return attrs.SingleOrDefault() as TAttribute;
                else
                    throw new SdkException($"More than one Attribute of Type '{typeof(TAttribute)}' is given");
            }

            return default;
        }
    }
}
