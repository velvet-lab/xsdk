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

using System.Text.Json;
using FluentValidation.Results;
using xSdk.Data.Mocks;

namespace xSdk.Data;

public sealed class ModelWithAdditionalDataTests
{
    [Fact]
    public void CreateModelWithoutAdditionalData()
    {
        var model = new TestModel
        {
            Name = "TestName"
        };

        Assert.Equal("TestName", model.Name);
    }

    [Fact]
    public void SerializeModelWithoutAdditionalData()
    {
        var model = new TestModel
        {
            Id = "1",
            Name = "TestName"
        };

        string json = JsonSerializer.Serialize(model);

        Assert.NotNull(json);
        Assert.Equal(@"{""Name"":""TestName"",""Id"":""1""}", json);
    }

    [Fact]
    public void DeserializeModelWithoutAdditionalData()
    {
        var model = new TestModel
        {
            Name = "TestName"
        };

        string json = JsonSerializer.Serialize(model);
        TestModel? actual = JsonSerializer.Deserialize<TestModel>(json);

        Assert.NotNull(actual);
        Assert.Equal(actual.Name, model.Name);
    }

    [Fact]
    public void DeserializeModelWithoutAdditionalDataWithValidation()
    {
        var model = new TestModel
        {
            Name = "TestName"
        };

        string json = JsonSerializer.Serialize(model);
        TestModel? actual = JsonSerializer.Deserialize<TestModel>(json);

        var validator = new TestModelValidation();
        ValidationResult result = validator.Validate(actual);

        Assert.NotNull(actual);
        Assert.Equal(actual.Name, model.Name);
        Assert.True(result.IsValid);
    }
}
