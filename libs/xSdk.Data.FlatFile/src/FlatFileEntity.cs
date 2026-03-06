using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace xSdk.Data
{
    public abstract class FlatFileEntity : Entity, IEntity<GuidPK, Guid>
    {
        public FlatFileEntity()
        {
            this.PrimaryKey = new GuidPK();
        }

        [Key]
        [Column("id")]
        [XmlElement("id")]
        [JsonPropertyName("id")]
        [SoapAttribute("id")]
        public new Guid Id
        {
            get => PrimaryKey.GetValue<Guid>();
            set => PrimaryKey.SetValue(value);
        }
    }
}
