/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
