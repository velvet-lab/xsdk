using System.ComponentModel.DataAnnotations;

namespace xSdk.Data.Mocks
{
    internal class TestEntity : Entity, IEntity<GuidPK, Guid>
    {
        public TestEntity()
        {
            this.PrimaryKey = new GuidPK();
        }

        [Key]
        public new Guid Id
        {
            get => PrimaryKey.GetValue<Guid>();
            set => PrimaryKey.SetValue(value);
        }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
