using Bogus;

namespace xSdk.Data.Mocks
{
    internal class TestEntityFakes : Fakes<TestEntity>
    {
        protected override void Build(Faker<TestEntity> builder)
        {
            builder
                .RuleFor(x => x.Key, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.Value, f => f.Person.FullName);
        }
    }
}
