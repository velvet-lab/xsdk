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

using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace xSdk;

public class SdkExceptionTests
{

    [Fact]
    public void DefaultConstructor_CreatesInstance()
    {
        var ex = new SdkException();

        Assert.NotNull(ex);
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        string message = "Test error message";

        var ex = new SdkException(message);

        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsBoth()
    {
        string message = "Outer error";
        var inner = new InvalidOperationException("Inner error");

        var ex = new SdkException(message, inner);

        Assert.Equal(message, ex.Message);
        Assert.Same(inner, ex.InnerException);
    }

    [Fact]
    public void SdkException_IsException()
    {
        var ex = new SdkException("Test");

        Assert.IsAssignableFrom<Exception>(ex);
    }

    [Fact]
    public void SdkException_CanBeThrown()
    {
        static void Throw() => throw new SdkException("thrown");
        Assert.Throws<SdkException>(Throw);
    }

    [Fact]
    public void SdkException_CanBeCaught_AsException()
    {
        Exception? caught = null;
        try
        {
            throw new SdkException("thrown");
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        Assert.NotNull(caught);
        Assert.IsType<SdkException>(caught);
    }
}
