using Agriventory.Model;
using MongoDB.Driver;

public class MongoDBService
{
    private readonly IMongoDatabase _database;

    public MongoDBService()
    {
        var client = new MongoClient("mongodb://127.0.0.1:27017");
        _database = client.GetDatabase("agriventory");
    }

    public IMongoCollection<User> GetUsersCollection()
    {
        return _database.GetCollection<User>("user");
    }
}
