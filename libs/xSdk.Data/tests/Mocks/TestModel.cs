using System.ComponentModel.DataAnnotations;

namespace xSdk.Data.Mocks
{
    internal class TestModel : Model, IModel<GuidStringPK, string>
    {
        public TestModel()
        {
            this.PrimaryKey = new GuidStringPK();
        }

        [Key]
        public new string Id
        {
            get => PrimaryKey.GetValue<string>();
            set => PrimaryKey.SetValue(value);
        }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}
