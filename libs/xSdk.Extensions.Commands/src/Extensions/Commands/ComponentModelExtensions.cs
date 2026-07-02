using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal static class ComponentModelExtensions
{
    extension(PropertyInfo property)
    {
        internal string? Description()
        {
            var attribute = property.GetAttribute<DescriptionAttribute>();
            return attribute?.Description;
        }

        internal bool IsRequired()
        {
            var attribute = property.GetAttribute<RequiredAttribute>();
            return attribute is not null;
        }
    }
}
