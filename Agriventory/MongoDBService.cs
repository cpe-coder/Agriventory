using Agriventory.Model;
using MongoDB.Driver;
using System.Threading.Tasks;
using Agriventory.Model;

public class MongoDBService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<ChickenItem> _chickenCollection;

    public MongoDBService()
    {
        var client = new MongoClient("mongodb://127.0.0.1:27017");
        _database = client.GetDatabase("agriventory");
        _chickenCollection = _database.GetCollection<ChickenItem>("chickens");
    }

    public IMongoCollection<User> GetUsersCollection()
    {
        return _database.GetCollection<User>("admin");
    }

    public async Task AddChickenAsync(ChickenItem chicken)
    {
        await _chickenCollection.InsertOneAsync(chicken);
    }

    public async Task<List<ChickenItem>> GetAllChickensAsync()
    {
        return await _chickenCollection.Find(_ => true).ToListAsync();
    }
    
    
}
