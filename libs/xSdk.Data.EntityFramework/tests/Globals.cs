using xSdk.Data.Mocks;

namespace xSdk.Data
{
    internal static class Globals
    {
        internal const string DatabaseName = "UnitTestDb";
        internal const string DatalayerName = "UniqueTestName";

        internal static TestEntity[] Entities = new TestEntity[]
        {
            new TestEntity
            {
                Id = Guid.Parse("653020ca-9144-4aa2-a85e-6c8357f58589"),
                Name = "Frodo",
                Age = 10,
            },
            new TestEntity
            {
                Id = Guid.Parse("1fa2fe66-ec2d-46c7-94ea-b852e4e4dd99"),
                Name = "Sam",
                Age = 314,
            },
            new TestEntity
            {
                Id = Guid.Parse("a01458ad-de5c-4a52-96b4-580c7729fd82"),
                Name = "Gandalf",
                Age = 4876,
            },
        };

        internal static ConcurrentEntityOne[] ConcurrentEntitiesOne = new ConcurrentEntityOne[]
        {
            new ConcurrentEntityOne
            {
                Id = Guid.Parse("d06326fa-b803-4b62-b9b1-10470e593d89"),
                Name = "Frodo",
                Age = 10,
            },
            new ConcurrentEntityOne
            {
                Id = Guid.Parse("77d50156-a42f-4ac9-91c6-07893ffb8411"),
                Name = "Sam",
                Age = 314,
            },
            new ConcurrentEntityOne
            {
                Id = Guid.Parse("f30b7d7e-4f09-4f36-9652-8fb59d0d373b"),
                Age = 4876,
                Name = "Gandalf",
            },
        };

        internal static ConcurrentEntityTwo[] ConcurrentEntitiesTwo = new ConcurrentEntityTwo[]
        {
            new ConcurrentEntityTwo
            {
                Id = Guid.Parse("e5555196-25db-4a98-b215-0e648878cdc9"),
                Name = "Frodo",
                Age = 10,
            },
            new ConcurrentEntityTwo
            {
                Id = Guid.Parse("ba7e217b-880e-421a-ac54-a58b9def9801"),
                Name = "Sam",
                Age = 314,
            }
        };
    }
}
