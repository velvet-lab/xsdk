using System.ComponentModel;
using xSdk.Data;

namespace xSdk.Demos.Data;

[Description("A sample model")]
public sealed class SampleModel : Model, IModel<GuidStringPK, string>
{
    public SampleModel()
    {
        this.PrimaryKey = new GuidStringPK();
    }

    [Description("The id of the sample model")]
    public new string Id
    {
        get => PrimaryKey.GetValue<string>();
        set => PrimaryKey.SetValue(value);
    }

    [Description("The name of the sample model")]
    public string Name { get; set; }
}
