using LiteDB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace xSdk.Data
{
    public abstract class NoSqlEntity : Entity, IEntity<NoSqlEntityPK, ObjectId>
    {
        public NoSqlEntity()
        {
            this.PrimaryKey = new NoSqlEntityPK();
        }

        [BsonId(true)]
        [Key]
        [Column("id")]
        [XmlElement("id")]
        [JsonPropertyName("id")]
        [SoapAttribute("id")]
        public new ObjectId Id
        {
            get => PrimaryKey.GetValue<ObjectId>();
            set => PrimaryKey.SetValue(value);
        }
    }
}
