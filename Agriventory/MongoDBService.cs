using Agriventory.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Agriventory;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<ChickenItem> _chickenCollection;
    private readonly IMongoCollection<PigItem> _pigCollection;
    private readonly IMongoCollection<DeliveryChickenItem> _deliveryChickenCollection;
    private readonly IMongoCollection<DeliveryPigItem> _deliveryPigCollection;
    public MongoDbService()
    {
        var client = new MongoClient("mongodb://127.0.0.1:27017");
        _database = client.GetDatabase("agriventory");
        _chickenCollection = _database.GetCollection<ChickenItem>("chickens");
        _pigCollection = _database.GetCollection<PigItem>("pigs");
        _deliveryPigCollection =  _database.GetCollection<DeliveryPigItem>("deliveryPigs");
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
        var sortDefinition = Builders<ChickenItem>.Sort.Ascending(c => c.DateUpdated);
        return await _chickenCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
    }
    public async Task UpdateChickenAsync(ChickenItem item)
    {
        var filter = Builders<ChickenItem>.Filter.Eq("_id", ObjectId.Parse(item.Id));
        var update = Builders<ChickenItem>.Update
            .Set(x => x.ProductName, item.ProductName)
            .Set(x => x.Stocks, item.Stocks)
            .Set(x => x.Brand, item.Brand)
            .Set(x => x.DateUpdated, item.DateUpdated);

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
    
    
    public async Task AddPigAsync(PigItem pig)
    {
        await _pigCollection.InsertOneAsync(pig);
    }
    
    
    public async Task<List<PigItem>> GetAllPigsAsync()
    {
        var sortDefinition = Builders<PigItem>.Sort.Ascending(c => c.DateUpdated);
        return await _pigCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
    }
    public async Task UpdatePigAsync(PigItem item)
    {
        var filter = Builders<PigItem>.Filter.Eq("_id", ObjectId.Parse(item.Id));
        var update = Builders<PigItem>.Update
            .Set(x => x.ProductName, item.ProductName)
            .Set(x => x.Stocks, item.Stocks)
            .Set(x => x.Brand, item.Brand)
            .Set(x => x.DateUpdated, item.DateUpdated);

        var result = await _pigCollection.UpdateOneAsync(filter, update);

        if (result.MatchedCount == 0)
        {
            throw new Exception("No record found to update.");
        }
    }
    public async Task DeletePigAsync(string? id)
    {
        var filter = Builders<PigItem>.Filter.Eq("_id", ObjectId.Parse(id));
        var result = await _pigCollection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
        {
            throw new Exception("No record found to delete.");
        }
    }
    
    public async Task DeliveryPigAsync(DeliveryPigItem deliveryPig)
    {
        await _deliveryPigCollection.InsertOneAsync(deliveryPig);
    }
}