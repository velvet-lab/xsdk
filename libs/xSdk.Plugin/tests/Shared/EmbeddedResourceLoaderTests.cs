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

using System.Reflection;
using xSdk.Shared;

namespace xSdk.Plugin.Tests.Shared;

public class EmbeddedResourceLoaderTests
{
    private static readonly Assembly _testAssembly = Assembly.GetExecutingAssembly();
    private const string TestNamespace = "xSdk";

    [Fact]
    public void TryReadResource_ExistingResource_ReturnsTrueAndContent()
    {
        var loader = new EmbeddedResourceLoader(_testAssembly, TestNamespace);

        var found = loader.TryReadResource("Resources/test.txt", out string content);

        Assert.True(found);
        Assert.Contains("Hello from embedded resource", content);
    }

    [Fact]
    public void TryReadResource_NonExistentResource_ReturnsFalse()
    {
        var loader = new EmbeddedResourceLoader(_testAssembly, TestNamespace);

        var found = loader.TryReadResource("does-not-exist.txt", out string content);

        Assert.False(found);
        Assert.Null(content);
    }

    [Fact]
    public void TryReadBinaryResource_ExistingResource_ReturnsTrueAndBytes()
    {
        var loader = new EmbeddedResourceLoader(_testAssembly, TestNamespace);

        var found = loader.TryReadBinaryResource("Resources/test.txt", out byte[] buffer);

        Assert.True(found);
        Assert.NotEmpty(buffer);
    }

    [Fact]
    public void TryReadBinaryResource_NonExistentResource_ReturnsFalse()
    {
        var loader = new EmbeddedResourceLoader(_testAssembly, TestNamespace);

        var found = loader.TryReadBinaryResource("does-not-exist.bin", out byte[] buffer);

        Assert.False(found);
        Assert.Empty(buffer);
    }

    [Fact]
    public void TryReadResource_WrongNamespace_ReturnsFalse()
    {
        var loader = new EmbeddedResourceLoader(_testAssembly, "Wrong.Namespace");

        var found = loader.TryReadResource("Resources/test.txt", out string content);

        Assert.False(found);
        Assert.Null(content);
    }
}
