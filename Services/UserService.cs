using App_Harvest_API.Repo;
using App_Harvest_API.Models;
using MongoDB.Driver;
using MongoDB.Bson;


namespace App_Harvest_API.Services;

public class UserService
{
    public static Task<User?> getUser(string id)
    {
        MongoHelper db = new();

        return db.GetUser(id);
    }

    public static Task<List<User>> getAll()
    {
        MongoHelper db = new();

        return db.getAll();
    }

    public static Task<UpdateResult?> createUser(User user)
    {
        MongoHelper db = new();

        return db.createUser(user);
    }
}
