using System.ComponentModel.DataAnnotations.Schema;
using xSdk.Data;

namespace xSdk.Demos.Data;

[Table("sample")]
public sealed class SampleEntity : NoSqlEntity, ISampleEntity
{
    [Column("name")]
    public string Name { get; set; }

    [Column("age")]
    public int Age { get; set; }
}
