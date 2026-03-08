using Bogus;

namespace xSdk.Data.Mocks;

internal class TestEntityFakes : Fakes<TestEntity>
{
    protected override void Build(Faker<TestEntity> builder)
    {
        builder.RuleFor(x => x.Name, f => f.Person.FirstName).RuleFor(x => x.Age, f => f.Random.Number(1, 100));
    }
}
