using xSdk.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace xSdk.Demos.Data
{
    [SwaggerSchema("A sample model")]
    public sealed class SampleModel : Model, IModel<GuidStringPK, string>
    {
        public SampleModel()
        {
            this.PrimaryKey = new GuidStringPK();
        }

        [SwaggerSchema("The id of the sample model")]
        public new string Id
        {
            get => PrimaryKey.GetValue<string>();
            set => PrimaryKey.SetValue(value);
        }

        [SwaggerSchema("The name of the sample model")]
        public string Name { get; set; }
    }
}
