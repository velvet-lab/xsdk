using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Logging;

internal readonly struct DisplayNameOptions(bool fullName, bool includeGenericParameterNames, bool includeGenericParameters, char nestedTypeDelimiter)
{
    public bool FullName { get; } = fullName;

    public bool IncludeGenericParameters { get; } = includeGenericParameters;

    public bool IncludeGenericParameterNames { get; } = includeGenericParameterNames;

    public char NestedTypeDelimiter { get; } = nestedTypeDelimiter;
}
