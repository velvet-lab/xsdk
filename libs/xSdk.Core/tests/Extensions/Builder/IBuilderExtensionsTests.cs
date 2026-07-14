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

namespace xSdk.Extensions.Builder;

public class IBuilderExtensionsTests
{
    private sealed class ConcreteBuilder : BuilderBase
    {
    }

    private sealed class AnotherBuilder : BuilderBase
    {
    }

    [Fact]
    public void AsBuilder_SameType_ReturnsInstance()
    {
        IBuilder builder = new ConcreteBuilder();

        ConcreteBuilder result = builder.AsBuilder<ConcreteBuilder>();

        Assert.Same(builder, result);
    }

    [Fact]
    public void AsBuilder_WrongType_ThrowsInvalidOperationException()
    {
        IBuilder builder = new ConcreteBuilder();

        Assert.Throws<InvalidOperationException>(() => builder.AsBuilder<AnotherBuilder>());
    }

    [Fact]
    public void AsBuilder_WrongType_ExceptionMessageContainsTypeName()
    {
        IBuilder builder = new ConcreteBuilder();

        var ex = Assert.Throws<InvalidOperationException>(() => builder.AsBuilder<AnotherBuilder>());

        Assert.Contains(typeof(AnotherBuilder).FullName!, ex.Message);
    }
}
