using App_Harvest_API.Repo;
using App_Harvest_API.Models;


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
}