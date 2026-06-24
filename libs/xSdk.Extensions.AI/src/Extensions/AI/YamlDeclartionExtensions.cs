using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Agents.ObjectModel;

namespace xSdk.Extensions.AI;

internal static class YamlDeclartionExtensions
{
    extension(RecordDataValue data)
    {
        internal bool TryReadValue(string path, out string? value)
        {
            var propertyPath = PropertyPath.Create(path);
            StringDataValue? property = data.GetProperty<StringDataValue>(propertyPath);

            if (property is not null)
            {
                value = property.Value;
                return true;
            }

            value = default;
            return false;
        }

        internal string? ReadValue(string path)
        {
            data.TryReadValue(path, out string? value);
            return value;
        }
    }
}
