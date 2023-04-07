using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

using System.Diagnostics;
using App_Harvest_API.Models;


namespace App_Harvest_API.Repo;


public class MongoHelper 
{
    IMongoDatabase db;
    GridFSBucket bucket;
    IMongoCollection<User> userCollection;
    IMongoCollection<IFormFile> contentCollection;

    public MongoHelper()
    {
        
        var settings = MongoClientSettings.FromConnectionString("mongodb+srv://App-Harvest:App-Harvest-Password@app-harvest.9e6gn6x.mongodb.net/?retryWrites=true&w=majority");
        var client = new MongoClient(settings);
        db = client.GetDatabase("App-Harvest");
        userCollection = db.GetCollection<User>("Users");
        CreateBucket();

    }

    public void CreateBucket()
    {
	   
	    var options = new GridFSBucketOptions
	    {
		    BucketName ="testBucket",
		    ChunkSizeBytes = 255 * 1024 //255 MB is the default value
	    };
	    bucket = new(db, options);
    }


    public async Task UploadAsync(IFormFile file)
    {
      var type = file.ContentType.ToString();
      var fileName = file.FileName;

      var options = new GridFSUploadOptions
      {
          Metadata = new BsonDocument { { "FileName", fileName }, { "Type", type } }
      };

      using var stream = await bucket.OpenUploadStreamAsync(fileName, options); // Open the output stream
      var id = stream.Id; // Unique Id of the file
      file.CopyTo(stream); // Copy the contents to the stream
      await stream.CloseAsync(); 
    }

    public async Task<byte[]> GetFileByNameAsync(string fileName)
    {
        return await bucket.DownloadAsBytesByNameAsync(fileName);
    }


//Method 2

    public async Task<byte[]> GetFileByIdAsync(string fileName)
    {
        var fileInfo = await FindFile(fileName);
        return await bucket.DownloadAsBytesAsync(fileInfo.Id);
    }

    private async Task<GridFSFileInfo> FindFile(string fileName)
    {
        var options = new GridFSFindOptions
        {
            Limit = 1
        };
        var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
        using var cursor = await bucket.FindAsync(filter, options);
        return (await cursor.ToListAsync()).FirstOrDefault();
    }

    public async Task<User?> GetUser(string email)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var filter = Builders<User>.Filter.Eq(x => x.email, email);
        var query = userCollection.Find(user => user.email == email);
        User? result = await query.FirstOrDefaultAsync();
        return result;

    }

    public async Task<DeleteResult> deleteUser(string email)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var filter1 = Builders<User>.Filter.Eq("email", email);
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
        var filter = Builders<User>.Filter.Eq(u => u.email, user.email);
        var update = Builders<User>.Update.Set(u => u.Name, user.Name)
                                      .Set(u => u.ContentCodes, user.ContentCodes)
                                      .Set(u => u.password, user.password);
        var options = new UpdateOptions { IsUpsert = true };
        var result = await userCollection.UpdateOneAsync(filter, update, options);
        return result;
  
    }

}