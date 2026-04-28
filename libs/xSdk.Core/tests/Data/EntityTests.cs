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

public class EntityTests
{
    private sealed class IntEntity : Entity<int>
    {
    }

    private sealed class GuidEntity : Entity<Guid>
    {
    }

    private sealed class StringEntity : Entity<string>
    {
    }

    [Fact]
    public void Entity_IntId_CanBeSetAndRead()
    {
        var entity = new IntEntity();

        entity.Id = 42;

        Assert.Equal(42, entity.Id);
    }

    [Fact]
    public void Entity_GuidId_CanBeSetAndRead()
    {
        var entity = new GuidEntity();
        var id = Guid.NewGuid();

        entity.Id = id;

        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Entity_StringId_CanBeSetAndRead()
    {
        var entity = new StringEntity();

        entity.Id = "abc-123";

        Assert.Equal("abc-123", entity.Id);
    }

    [Fact]
    public void Entity_DefaultIntId_IsZero()
    {
        var entity = new IntEntity();

        Assert.Equal(0, entity.Id);
    }

    [Fact]
    public void Entity_DefaultGuidId_IsEmpty()
    {
        var entity = new GuidEntity();

        Assert.Equal(Guid.Empty, entity.Id);
    }
}

public class ModelTests
{
    private sealed class ConcreteModel : Model
    {
    }

    [Fact]
    public void Model_Id_CanBeSetAndRead()
    {
        var model = new ConcreteModel();

        model.Id = "test-id";

        Assert.Equal("test-id", model.Id);
    }

    [Fact]
    public void Model_Id_DefaultIsNull()
    {
        var model = new ConcreteModel();

        Assert.Null(model.Id);
    }

    [Fact]
    public void Model_AdditionalData_CanBeSetAndRead()
    {
        var model = new ConcreteModel();
        var data = new Dictionary<string, object> { ["key"] = "value" };

        model.AdditionalData = data;

        Assert.Equal("value", model.AdditionalData["key"]);
    }

    [Fact]
    public void Model_AdditionalData_DefaultIsNull()
    {
        var model = new ConcreteModel();

        Assert.Null(model.AdditionalData);
    }
}
