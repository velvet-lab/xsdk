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
using xSdk.Tools;

namespace xSdk.Data;

public class EFModelTests
{
    [Fact]
    public void CreateNewModel()
    {
        var model = new TestModel();
        model.Id = PrimaryKeyTools.Generate<Guid>().ToString();

        Assert.NotNull(model);
        Assert.NotNull(model.Id);
        Assert.Equal(model.Id, model.Id);
        Assert.IsType<string>(model.Id);
    }

    [Fact]
    public void CreateNewEntityFromExistingPrimaryKey()
    {
        var pk = Guid.NewGuid().ToString();
        var model = new TestModel();
        model.Id = pk;

        Assert.NotNull(model);
        Assert.NotNull(model.Id);
        Assert.Equal(model.Id, model.Id);
        Assert.IsType<string>(model.Id);
    }

    [Fact]
    public void CreateNewEntityByInterface()
    {
        var pk = Guid.NewGuid().ToString();
        IModel model = new TestModel();
        model.Id = pk;

        Assert.NotNull(model);
        Assert.NotNull(model.Id);
        Assert.Equal(model.Id, model.Id);
        Assert.IsType<string>(model.Id);
    }
}
