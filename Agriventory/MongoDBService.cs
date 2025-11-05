using Agriventory.Model;
using MongoDB.Driver;
using MongoDB.Bson;

public class MongoDBService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<ChickenItem> _chickenCollection;
    private readonly IMongoCollection<DeliveryChickenItem> _deliveryChickenCollection;
    public MongoDBService()
    {
        var client = new MongoClient("mongodb://127.0.0.1:27017");
        _database = client.GetDatabase("agriventory");
        _chickenCollection = _database.GetCollection<ChickenItem>("chickens");
        _deliveryChickenCollection = _database.GetCollection<DeliveryChickenItem>("deliveryChickens");
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
        var sortDefinition = Builders<ChickenItem>.Sort.Ascending(c => c.DateImported);
        return await _chickenCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
    }
    public async Task UpdateChickenAsync(ChickenItem item)
    {
        var filter = Builders<ChickenItem>.Filter.Eq("_id", ObjectId.Parse(item.Id));
        var update = Builders<ChickenItem>.Update
            .Set(x => x.ProductName, item.ProductName)
            .Set(x => x.Stocks, item.Stocks)
            .Set(x => x.Brand, item.Brand)
            .Set(x => x.DateImported, item.DateImported);

        var result = await _chickenCollection.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0)
        {
            throw new Exception("No record found to update.");
        }
    }
    public async Task DeleteChickenAsync(string? id)
    {
        var filter = Builders<ChickenItem>.Filter.Eq("_id", ObjectId.Parse(id));
        var result = await _chickenCollection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
        {
            throw new Exception("No record found to delete.");
        }
    }
    
    public async Task DeliveryChickenAsync(DeliveryChickenItem deliveryChicken)
    {
        await _deliveryChickenCollection.InsertOneAsync(deliveryChicken);
    }
    
}
