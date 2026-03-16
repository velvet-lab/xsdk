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

using Bogus;
using MongoDB.Bson;

namespace xSdk.Data.Mocks;

internal class TestEntityFakes : Fakes<TestEntity>
{
    protected override void Build(Faker<TestEntity> builder)
    {
        builder
            .RuleFor(x => x.Id, f => ObjectId.GenerateNewId())
            .RuleFor(x => x.Age, f => f.Random.Number(1, 100))
            .RuleFor(x => x.Name, f => f.Person.FullName);
    }
}
