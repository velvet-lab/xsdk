using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace xSdk.Data
{
    public abstract class EFEntity : Entity, IEntity<GuidPK, Guid>
    {
        protected EFEntity()
        {
            this.PrimaryKey = new GuidPK();
        }

        [Key]
        [Column("id")]
        [XmlElement("id")]
        [JsonPropertyName("id")]
        public new Guid Id
        {
            get => PrimaryKey.GetValue<Guid>();
            set => PrimaryKey.SetValue(value);
        }
    }
}
