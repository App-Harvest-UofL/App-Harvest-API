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
        
        var settings = MongoClientSettings.FromConnectionString("mongodb+srv://App-Harvest:App-Harvest-Password@app-harvest.9e6gn6x.mongodb.net/?retryWrites=true&w=majority");
        var client = new MongoClient(settings);
        db = client.GetDatabase("App-Harvest");
        userCollection = db.GetCollection<User>("Users");


    }

    public async Task<User?> GetUser(string id)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var query = userCollection.Find(user => user.Id == id);
        User? result = await query.FirstOrDefaultAsync();
        return result;

    }

    public async Task<DeleteResult> deleteUser(string id)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var filter1 = Builders<User>.Filter.Eq("_id", id);
        var result = await userCollection.DeleteOneAsync(filter1); 
        return result;
    }

    public async Task<List<User>> getAll()
    {
        Stopwatch sw = Stopwatch.StartNew();
        var results = await userCollection.Find(new BsonDocument()).ToListAsync();
        return results;
    }

    public async Task<UpdateResult?> createUser(User user)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
        var update = Builders<User>.Update.Set(u => u.Name, user.Name)
                                      .Set(u => u.email, user.email)
                                      .Set(u => u.ContentCodes, user.ContentCodes)
                                      .Set(u => u.Password, user.Password);
        var options = new UpdateOptions { IsUpsert = true };
        var result = await userCollection.UpdateOneAsync(filter, update, options);
        return result;
  
    }

}