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

using xSdk.Data.Mocks;

namespace xSdk.Data;

public class FakerTests
{
    [Fact]
    public void CreateFakes()
    {
        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>();

        Assert.NotNull(entity);
        Assert.NotNull(entity.Id);
        Assert.NotNull(entity.Name);
        Assert.True(entity.Age > 0);
    }

    [Fact]
    public void Generate_WithContext_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>("default");

        Assert.NotNull(entity);
    }

    [Fact]
    public void Generate_WithStrictMode_ReturnsEntity()
    {
        var entity = FakeGenerator.Generate<TestEntityFakes, TestEntity>(true);

        Assert.NotNull(entity);
    }

    [Fact]
    public void Fakes_GetExamples_ReturnsEntity()
    {
        var fakes = new TestEntityFakes();
        var entity = fakes.GetExamples();

        Assert.NotNull(entity);
    }
}
