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

public class FlatFileEntityTests
{
    private class TestEntity : FlatFileEntity
    {
        public string Label { get; set; } = string.Empty;
    }

    [Fact]
    public void FlatFileEntity_DefaultConstructor_InitializesPrimaryKey()
    {
        var entity = new TestEntity();

        Assert.NotNull(entity.PrimaryKey);
        Assert.IsType<GuidPK>(entity.PrimaryKey);
    }

    [Fact]
    public void FlatFileEntity_IdProperty_GetSet_WorksCorrectly()
    {
        var entity = new TestEntity();
        var id = Guid.NewGuid();

        entity.Id = id;

        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void FlatFileEntity_IdDefault_IsNotEmptyGuid()
    {
        var entity = new TestEntity();

        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void FlatFileEntity_CustomProperties_CanBeSetAndRetrieved()
    {
        var entity = new TestEntity { Label = "My Label" };

        Assert.Equal("My Label", entity.Label);
    }

    [Fact]
    public void FlatFileEntity_TwoEntities_HaveDifferentDefaultIds()
    {
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();
        entity1.Id = Guid.NewGuid();
        entity2.Id = Guid.NewGuid();

        Assert.NotEqual(entity1.Id, entity2.Id);
    }
}
