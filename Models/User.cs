using System.Collections.Generic;
using App_Harvest_API.Repo; 
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace App_Harvest_API.Models;

[BsonIgnoreExtraElements]
public class User
{

    [BsonElement("_id")]
    [BsonId]
    public string? Id { get; set; }

    public string Name { get; set; } = null!;

    public string email { get; set; } = null!;

    public string ContentCodes { get; set; } = null!;
    public string Password { get; set; } = null!;
}