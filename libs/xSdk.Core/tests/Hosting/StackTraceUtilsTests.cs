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

public class StackTraceUtilsTests
{
    [Fact]
    public void GetFrameCount_ReturnsCorrectCount()
    {
        var stackTrace = new StackTrace();

        var count = stackTrace.GetFrameCount();

        Assert.True(count > 0);
    }

    [Fact]
    public void GetStackFrameMethodName_NullMethod_ReturnsEmpty()
    {
        var result = StackTraceUtils.GetStackFrameMethodName(null, false, false, false);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetStackFrameMethodName_RegularMethod_ReturnsName()
    {
        var method = typeof(StackTraceUtilsTests).GetMethod(nameof(GetStackFrameMethodName_RegularMethod_ReturnsName))!;

        var result = StackTraceUtils.GetStackFrameMethodName(method, false, false, false);

        Assert.Equal(nameof(GetStackFrameMethodName_RegularMethod_ReturnsName), result);
    }

    [Fact]
    public void GetStackFrameMethodName_WithIncludeMethodInfo_ReturnsFullSignature()
    {
        var method = typeof(StackTraceUtilsTests).GetMethod(nameof(GetStackFrameMethodName_WithIncludeMethodInfo_ReturnsFullSignature))!;

        var result = StackTraceUtils.GetStackFrameMethodName(method, includeMethodInfo: true, cleanAsyncMoveNext: false, cleanAnonymousDelegates: false);

        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetStackFrameMethodClassName_NullMethod_ReturnsEmpty()
    {
        var result = StackTraceUtils.GetStackFrameMethodClassName(null, false, false, false);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetStackFrameMethodClassName_WithNamespace_ReturnsFullName()
    {
        var method = typeof(StackTraceUtilsTests).GetMethod(nameof(GetStackFrameMethodClassName_WithNamespace_ReturnsFullName))!;

        var result = StackTraceUtils.GetStackFrameMethodClassName(method, includeNameSpace: true, cleanAsyncMoveNext: false, cleanAnonymousDelegates: false);

        Assert.Contains("xSdk", result);
    }

    [Fact]
    public void GetStackFrameMethodClassName_WithoutNamespace_ReturnsShortName()
    {
        var method = typeof(StackTraceUtilsTests).GetMethod(nameof(GetStackFrameMethodClassName_WithoutNamespace_ReturnsShortName))!;

        var result = StackTraceUtils.GetStackFrameMethodClassName(method, includeNameSpace: false, cleanAsyncMoveNext: false, cleanAnonymousDelegates: false);

        Assert.Equal(nameof(StackTraceUtilsTests), result);
    }

    [Fact]
    public void GetStackMethod_ValidStackFrame_ReturnsMethod()
    {
        var stackTrace = new StackTrace(false);
        var frame = stackTrace.GetFrame(0);

        var method = StackTraceUtils.GetStackMethod(frame);

        Assert.NotNull(method);
    }

    [Fact]
    public void GetStackMethod_NullFrame_ReturnsNull()
    {
        var method = StackTraceUtils.GetStackMethod(null);

        Assert.Null(method);
    }

    [Fact]
    public void GetClassFullName_ValidStackFrame_ReturnsNonEmpty()
    {
        var stackTrace = new StackTrace(false);
        var frame = stackTrace.GetFrame(0);

        var className = StackTraceUtils.GetClassFullName(frame);

        Assert.NotEmpty(className);
    }

    [Fact]
    public void GetClassFullName_NullFrame_StillReturnsResult()
    {
        var className = StackTraceUtils.GetClassFullName(null);

        // Should return non-empty via fallback to StackTrace
        Assert.NotNull(className);
    }

    [Fact]
    public void LookupAssemblyFromMethod_SystemType_ReturnsNull()
    {
        var method = typeof(string).GetMethod("ToString", Type.EmptyTypes)!;

        var assembly = StackTraceUtils.LookupAssemblyFromMethod(method);

        Assert.Null(assembly);
    }

    [Fact]
    public void LookupAssemblyFromMethod_UserType_ReturnsAssembly()
    {
        var method = typeof(StackTraceUtilsTests).GetMethod(nameof(LookupAssemblyFromMethod_UserType_ReturnsAssembly))!;

        var assembly = StackTraceUtils.LookupAssemblyFromMethod(method);

        Assert.NotNull(assembly);
    }

    [Fact]
    public void LookupClassNameFromStackFrame_ValidFrame_ReturnsNonEmpty()
    {
        var stackTrace = new StackTrace(false);
        var frame = stackTrace.GetFrame(0);

        var className = StackTraceUtils.LookupClassNameFromStackFrame(frame);

        Assert.NotEmpty(className);
    }

    [Fact]
    public void LookupClassNameFromStackFrame_NullFrame_ReturnsEmpty()
    {
        var className = StackTraceUtils.LookupClassNameFromStackFrame(null);

        Assert.Equal(string.Empty, className);
    }
}
