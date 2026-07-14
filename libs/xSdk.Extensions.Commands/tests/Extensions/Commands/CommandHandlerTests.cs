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

namespace xSdk.Extensions.Commands;

public class CommandHandlerTests
{
    private sealed class SimpleHandler : CommandHandler
    {
    }

    private sealed class OverriddenAsyncHandler : CommandHandler
    {
        public override Task<int> ExecuteAsync(CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    private sealed class OverriddenSyncHandler : CommandHandler
    {
        public override int Execute() => 99;
    }

    [Fact]
    public void Execute_BaseImplementation_ReturnsZero()
    {
        var handler = new SimpleHandler();

        int result = handler.Execute();

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task ExecuteAsync_BaseImplementation_ReturnsZero()
    {
        var handler = new SimpleHandler();

        int result = await handler.ExecuteAsync(CancellationToken.None);

        Assert.Equal(0, result);
    }

    [Fact]
    public void IsAsyncOverridden_BaseHandler_ReturnsFalse()
    {
        var handler = new SimpleHandler();

        Assert.False(handler.IsAsyncOverridden);
    }

    [Fact]
    public void IsAsyncOverridden_DerivedHandlerWithAsyncOverride_ReturnsTrue()
    {
        var handler = new OverriddenAsyncHandler();

        Assert.True(handler.IsAsyncOverridden);
    }

    [Fact]
    public void IsAsyncOverridden_DerivedHandlerWithSyncOverrideOnly_ReturnsFalse()
    {
        var handler = new OverriddenSyncHandler();

        Assert.False(handler.IsAsyncOverridden);
    }

    [Fact]
    public async Task ExecuteAsync_OverriddenHandler_ReturnsExpectedValue()
    {
        var handler = new OverriddenAsyncHandler();

        int result = await handler.ExecuteAsync(CancellationToken.None);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Execute_OverriddenHandler_ReturnsExpectedValue()
    {
        var handler = new OverriddenSyncHandler();

        int result = handler.Execute();

        Assert.Equal(99, result);
    }

    [Fact]
    public void Context_Default_IsNull()
    {
        var handler = new SimpleHandler();

        Assert.Null(handler.Context);
    }
}
