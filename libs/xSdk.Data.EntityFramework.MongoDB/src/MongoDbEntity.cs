using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace xSdk.Data;

public abstract class MongoDbEntity : Entity, IEntity<MongoDbEntityPK, ObjectId>
{
    protected MongoDbEntity()
    {
        this.PrimaryKey = new MongoDbEntityPK();
    }

    [Key]
    [Column("_id")]
    [XmlElement("_id")]
    [JsonPropertyName("_id")]
    [BsonElement("_id")]
    [BsonId]
    public new ObjectId Id
    {
        get => PrimaryKey.GetValue<ObjectId>();
        set => PrimaryKey.SetValue(value);
    }
}
