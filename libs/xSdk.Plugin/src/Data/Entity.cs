using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Mapster;
using MongoDB.Bson.Serialization.Attributes;

namespace xSdk.Data;

public abstract class Entity : IEntity
{
    [NotMapped]
    [IgnoreDataMember]
    [JsonIgnore]
    [XmlIgnore]
    [SoapIgnore]
    [BsonIgnore]
    [AdaptIgnoreAttribute]
    public IPrimaryKey PrimaryKey { get; protected set; }

    [NotMapped]
    [IgnoreDataMember]
    [JsonIgnore]
    [XmlIgnore]
    [SoapIgnore]
    [BsonIgnore]
    [AdaptIgnoreAttribute]
    public object Id
    {
        get => PrimaryKey.GetValue();
        set => PrimaryKey.SetValue(value);
    }
}
