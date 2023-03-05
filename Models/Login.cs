using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App_Harvest_API.Models;

[BsonIgnoreExtraElements]
public class Login
{

    [BsonElement("_id")]
    [BsonId]
    public string? Username { get; set; }

    public string? Password { get; set; } 
}
