using App_Harvest_API.Repo;
using App_Harvest_API.Models;
using MongoDB.Driver;
using MongoDB.Bson;


namespace App_Harvest_API.Services;

public class UserService
{
    public static Task<User?> getUser(string email)
    {
        MongoHelper db = new();

        var result =  db.GetUser(email);
        if (result?.Result?.password != null)
        {
            result.Result.password = DecodeFrom64(result.Result.password);
            return result;
        }
        return null;
    }

    public static Task<DeleteResult> deleteUser(string email)
    {
        MongoHelper db = new();

        return  db.deleteUser(email);
    }

    public static Task<List<User>> getAll()
    {
        MongoHelper db = new();

        return db.getAll();
    }
        
    public static Task<UpdateResult?> createUser(User user)
    {
        MongoHelper db = new();
        if (user.password != null)
        {
            user.password = EncodePasswordToBase64(user.password);
        }
        return db.createUser(user);
    }
    //Reset a users password by finding the user and calling createUser() to set oldpassword=newpassword
    public static async Task<User?> userReset(string email, string newPassword)
    {
        MongoHelper db = new();

        var user = await db.GetUser(email);

        if (user == null)
        {
            return null;
        }

        user.password = EncodePasswordToBase64(newPassword);

        var result = await db.createUser(user);

        if (result == null)
        {
            return null;
        }

        return user;
    }
    //this function Convert to Encord your Password
    public static string EncodePasswordToBase64(string password)
    {
        try
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    }
    //this function Convert to Decord your Password
    public static string DecodeFrom64(string encodedData)
    {
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        System.Text.Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encodedData);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        string result = new String(decoded_char);
        return result;
    }
}
