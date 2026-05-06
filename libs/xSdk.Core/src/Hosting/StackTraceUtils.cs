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

using System.Diagnostics;
using System.Reflection;

namespace xSdk.Hosting;

internal static class StackTraceUtils
{
    private static readonly Assembly nlogAssembly = typeof(StackTraceUtils).Assembly;
    private static readonly Assembly mscorlibAssembly = typeof(string).Assembly;
    private static readonly Assembly systemAssembly = typeof(Debug).Assembly;

    public static int GetFrameCount(this StackTrace strackTrace)
    {
        return strackTrace.FrameCount;
    }

    public static string GetStackFrameMethodName(MethodBase? method, bool includeMethodInfo, bool cleanAsyncMoveNext, bool cleanAnonymousDelegates)
    {
        if (method is null)
        {
            return string.Empty;
        }

        string methodName = method.Name;

        Type? callerClassType = method.DeclaringType;
        if (cleanAsyncMoveNext && methodName == "MoveNext" && callerClassType?.DeclaringType != null && callerClassType.Name.IndexOf('<') == 0)
        {
            int endIndex = callerClassType.Name.IndexOf('>', 1);
            if (endIndex > 1)
            {
                methodName = callerClassType.Name.Substring(1, endIndex - 1);
                if (methodName.IndexOf('<') == 0)
                {
                    methodName = methodName.Substring(1);    // Local functions, and anonymous-methods in Task.Run()
                }
            }
        }

        // Clean up the function name if it is an anonymous delegate
        // <.ctor>b__0
        // <Main>b__2
        if (cleanAnonymousDelegates && (methodName.IndexOf('<') == 0 && methodName.IndexOf("__", StringComparison.Ordinal) >= 0 && methodName.IndexOf('>') >= 0))
        {
            int startIndex = methodName.IndexOf('<') + 1;
            int endIndex = methodName.IndexOf('>');
            methodName = methodName.Substring(startIndex, endIndex - startIndex);
        }

        if (includeMethodInfo && methodName == method.Name)
        {
            methodName = method.ToString() ?? methodName;
        }

        return methodName;
    }

    public static string GetStackFrameMethodClassName(MethodBase? method, bool includeNameSpace, bool cleanAsyncMoveNext, bool cleanAnonymousDelegates)
    {
        if (method is null)
        {
            return string.Empty;
        }

        Type? callerClassType = method.DeclaringType;
        if (cleanAsyncMoveNext
          && method.Name == "MoveNext"
          && callerClassType?.DeclaringType != null
          && callerClassType.Name?.IndexOf('<') == 0
          && callerClassType.Name.IndexOf('>', 1) > 1)
        {
            callerClassType = callerClassType.DeclaringType;
        }

        if (callerClassType is null)
        {
            return string.Empty;
        }

        string? className = includeNameSpace ? callerClassType.FullName : callerClassType.Name;
        if (cleanAnonymousDelegates && className?.IndexOf("<>", StringComparison.Ordinal) >= 0)
        {
            if (!includeNameSpace && callerClassType.DeclaringType != null && callerClassType.IsNested)
            {
                className = callerClassType.DeclaringType.Name;
            }
            else
            {
                int index = className.IndexOf("+<>", StringComparison.Ordinal);
                if (index >= 0)
                {
                    className = className.Substring(0, index);
                }
            }
        }

        if (includeNameSpace && className?.IndexOf('.') == -1)
        {
            string typeNamespace = GetNamespaceFromTypeAssembly(callerClassType);
            className = string.IsNullOrEmpty(typeNamespace) ? className : string.Concat(typeNamespace, ".", className);
        }

        return className ?? string.Empty;
    }

    private static string GetNamespaceFromTypeAssembly(Type? callerClassType)
    {
        Assembly? classAssembly = callerClassType?.Assembly;
        if (classAssembly != null && classAssembly != mscorlibAssembly && classAssembly != systemAssembly)
        {
            string? assemblyFullName = classAssembly.FullName;
            if (assemblyFullName?.IndexOf(',') >= 0 && !assemblyFullName.StartsWith("System.", StringComparison.Ordinal) && !assemblyFullName.StartsWith("Microsoft.", StringComparison.Ordinal))
            {
                return assemblyFullName.Substring(0, assemblyFullName.IndexOf(','));
            }
        }

        return string.Empty;
    }

    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("Trimming - Allow callsite logic", "IL2026")]
    public static MethodBase? GetStackMethod(StackFrame? stackFrame)
        => stackFrame?.GetMethod();

    /// <summary>
    /// Gets the fully qualified name of the class invoking the calling method, including the
    /// namespace but not the assembly.
    /// </summary>
    /// <param name="stackFrame">StackFrame from the calling method</param>
    /// <returns>Fully qualified class name</returns>
    public static string GetClassFullName(StackFrame? stackFrame)
    {
        string className = LookupClassNameFromStackFrame(stackFrame);
        if (string.IsNullOrEmpty(className))
        {
            var stackTrace = new StackTrace(false);
            className = GetClassFullName(stackTrace);
            if (string.IsNullOrEmpty(className))
            {
                MethodBase? method = StackTraceUtils.GetStackMethod(stackFrame);
                className = method?.Name ?? string.Empty;
            }
        }

        return className;
    }

    private static string GetClassFullName(StackTrace stackTrace)
    {
        foreach (StackFrame frame in stackTrace.GetFrames())
        {
            string className = LookupClassNameFromStackFrame(frame);
            if (!string.IsNullOrEmpty(className))
            {
                return className;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Returns the assembly from the provided StackFrame (If not internal assembly)
    /// </summary>
    /// <returns>Valid assembly, or null if assembly was internal</returns>
    public static Assembly? LookupAssemblyFromMethod(MethodBase method)
    {
        Assembly? assembly = method.DeclaringType?.Assembly ?? method.Module?.Assembly;

        // skip stack frame if the method declaring type assembly is from hidden assemblies list
        if (assembly == nlogAssembly)
        {
            return null;
        }

        if (assembly == mscorlibAssembly)
        {
            return null;
        }

        if (assembly == systemAssembly)
        {
            return null;
        }

        return assembly;
    }

    /// <summary>
    /// Returns the classname from the provided StackFrame (If not from internal assembly)
    /// </summary>
    /// <param name="stackFrame"></param>
    /// <returns>Valid class name, or empty string if assembly was internal</returns>
    public static string LookupClassNameFromStackFrame(StackFrame? stackFrame)
    {
        MethodBase? method = GetStackMethod(stackFrame);
        if (method != null && LookupAssemblyFromMethod(method) != null)
        {
            string className = GetStackFrameMethodClassName(method, true, true, true);
            if (!string.IsNullOrEmpty(className))
            {
                if (!className.StartsWith("System.", StringComparison.Ordinal))
                {
                    return className;
                }
            }
            else
            {
                className = method.Name ?? string.Empty;
                if (className != "lambda_method" && className != "MoveNext")
                {
                    return className;
                }
            }
        }

        return string.Empty;
    }
}
