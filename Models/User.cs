using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App_Harvest_API.Models;

[BsonIgnoreExtraElements]
public class User
{

    [BsonElement("_id")]
    [BsonId]
    public string? email { get; set; }

    public string Name { get; set; } = null!;

    public string ContentCodes { get; set; } = null!;
    public string password { get; set; } = null!;
}