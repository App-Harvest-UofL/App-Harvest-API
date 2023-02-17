using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using App_Harvest_API.Models;


namespace App_Harvest_API.Repo;


public class MongoHelper 
{
    IMongoDatabase db;
    IMongoCollection<User> userCollection;

    public MongoHelper()
    {
        var client = new MongoClient(""); // add connection string 
        db = client.GetDatabase("AppHarvest");
        userCollection = db.GetCollection<User>("User");

    }

    public async Task<User?> GetUser(string id)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var query = userCollection.Find(user => user.Id == id);
        User? result = await query.FirstOrDefaultAsync();
        return result;

    }

    public async Task<List<User>> getAll()
    {
        Stopwatch sw = Stopwatch.StartNew();
        var results = await userCollection.Find(new BsonDocument()).ToListAsync();
        return results;
    }

}