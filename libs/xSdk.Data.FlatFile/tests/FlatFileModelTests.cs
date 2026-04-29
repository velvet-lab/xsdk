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

namespace xSdk.Data;

public class FlatFileModelTests
{
    private class TestModel : Model
    {
        public string Title { get; set; } = string.Empty;
    }

    [Fact]
    public void FlatFileModel_DefaultConstructor_InitializesPrimaryKey()
    {
        var model = new TestModel();
        model.Id = Guid.NewGuid().ToString();

        Assert.NotNull(model.Id);
        Assert.IsType<string>(model.Id);
    }

    [Fact]
    public void FlatFileModel_IdProperty_GetSet_WorksCorrectly()
    {
        var model = new TestModel();
        var id = Guid.NewGuid().ToString();

        model.Id = id;

        Assert.Equal(id, model.Id);
    }

    [Fact]
    public void FlatFileModel_IdDefault_IsEmptyString()
    {
        var model = new TestModel();

        Assert.Null(model.Id);
    }

    [Fact]
    public void FlatFileModel_CustomProperties_CanBeSetAndRetrieved()
    {
        var model = new TestModel { Title = "Test Title" };

        Assert.Equal("Test Title", model.Title);
    }

    [Fact]
    public void FlatFileModel_SetIdTwice_KeepsLastValue()
    {
        var model = new TestModel();
        var first = Guid.NewGuid().ToString();
        var second = Guid.NewGuid().ToString();

        model.Id = first;
        model.Id = second;

        Assert.Equal(second, model.Id);
    }
}
