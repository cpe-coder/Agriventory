using Agriventory.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Agriventory;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<ChickenItem> _chickenCollection;
    private readonly IMongoCollection<PigItem> _pigCollection;
    private readonly IMongoCollection<TransactionItem> _deliveryCollection;
    public MongoDbService()
    {
        
        var client = new MongoClient("mongodb://127.0.0.1:27017");
        _database = client.GetDatabase("agriventory");
        _chickenCollection = _database.GetCollection<ChickenItem>("chickens");
        _pigCollection = _database.GetCollection<PigItem>("pigs");
        _deliveryCollection =  _database.GetCollection<TransactionItem>("transactions");
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
    
    public async Task DeliveryPigAsync(TransactionItem deliveryPig)
    {
        await _deliveryCollection.InsertOneAsync(deliveryPig);
    }
    
    public async Task DeliveryChickenAsync(TransactionItem deliveryChicken)
    {
        await _deliveryCollection.InsertOneAsync(deliveryChicken);
    }

    
    public async Task<List<TransactionItem>> GetAllTransactionsAsync()
    {
        var sortDefinition = Builders<TransactionItem>.Sort.Ascending(c => c.DateOfDelivery);
        return await _deliveryCollection.Find(_ => true).Sort(sortDefinition).ToListAsync();
    }
    
 public async Task<int> GetTotalStocksAsync()
    {
        var chickens = await _chickenCollection.Find(_ => true).ToListAsync();
        var pigs = await _pigCollection.Find(_ => true).ToListAsync();
        
        var chickenStocks = chickens.Sum(c => c.Stocks);
        var pigStocks = pigs.Sum(p => p.Stocks);
        
        return chickenStocks + pigStocks;
    }
    
    public async Task<int> GetTotalTransactionsAsync()
    {
        return (int)await _deliveryCollection.CountDocumentsAsync(_ => true);
    }
    
    public async Task<int> GetTotalPigProductsAsync()
    {
        return (int)await _pigCollection.CountDocumentsAsync(_ => true);
    }
    
    public async Task<int> GetTotalChickenProductsAsync()
    {
        return (int)await _chickenCollection.CountDocumentsAsync(_ => true);
    }
    
    public async Task<Dictionary<string, int>> GetAllBrandsWithCountAsync()
    {
        var chickens = await _chickenCollection.Find(_ => true).ToListAsync();
        var pigs = await _pigCollection.Find(_ => true).ToListAsync();
        
        var brandCounts = new Dictionary<string, int>();
        
        foreach (var chicken in chickens.Where(chicken => !string.IsNullOrWhiteSpace(chicken.Brand)))
        {
            if (brandCounts.ContainsKey(chicken.Brand!))
                brandCounts[chicken.Brand!]++;
            else
                brandCounts[chicken.Brand!] = 1;
        }
        
        foreach (var pig in pigs.Where(pig => !string.IsNullOrWhiteSpace(pig.Brand)))
        {
            if (brandCounts.ContainsKey(pig.Brand!))
                brandCounts[pig.Brand!]++;
            else
                brandCounts[pig.Brand!] = 1;
        }
        
        return brandCounts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }
    
    public async Task<Dictionary<string, int>> GetChickenBrandsWithStocksAsync()
    {
        try
        {
            var chickens = await _chickenCollection.Find(_ => true).ToListAsync();
            
            var brandStocks = new Dictionary<string, int>();
            
            foreach (var chicken in chickens.Where(chicken => !string.IsNullOrWhiteSpace(chicken.Brand)))
            {
                if (brandStocks.ContainsKey(chicken.Brand!))
                    brandStocks[chicken.Brand!] += chicken.Stocks;
                else
                    brandStocks[chicken.Brand!] = chicken.Stocks;
            }
            
            return brandStocks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetChickenBrandsWithStocksAsync: {ex.Message}");
            return new Dictionary<string, int>();
        }
    }
    
    public async Task<Dictionary<string, int>> GetPigBrandsWithStocksAsync()
    {
        try
        {
            var pigs = await _pigCollection.Find(_ => true).ToListAsync();
            
            var brandStocks = new Dictionary<string, int>();
            
            foreach (var pig in pigs.Where(pig => !string.IsNullOrWhiteSpace(pig.Brand)))
            {
                if (brandStocks.ContainsKey(pig.Brand!))
                    brandStocks[pig.Brand!] += pig.Stocks;
                else
                    brandStocks[pig.Brand!] = pig.Stocks;
            }
            
            return brandStocks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetPigBrandsWithStocksAsync: {ex.Message}");
            return new Dictionary<string, int>();
        }
    }
    
    public async Task<int> GetDailyOrdersAsync()
    {
        var today = DateTime.Today;

        var filter = Builders<TransactionItem>.Filter.Gte(x => x.DateOfDelivery, today);

        return (int)await _deliveryCollection.CountDocumentsAsync(filter);
    }

    public async Task<int> GetWeeklyOrdersAsync()
    {
        var startOfWeek = DateTime.Today.AddDays(-7);

        var filter = Builders<TransactionItem>.Filter.Gte(x => x.DateOfDelivery, startOfWeek);

        return (int)await _deliveryCollection.CountDocumentsAsync(filter);
    }

    public async Task<int> GetMonthlyOrdersAsync()
    {
        var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        var filter = Builders<TransactionItem>.Filter.Gte(x => x.DateOfDelivery, startOfMonth);

        return (int)await _deliveryCollection.CountDocumentsAsync(filter);
    }

    public async Task<int> GetAnnualOrdersAsync()
    {
        var startOfYear = new DateTime(DateTime.Today.Year, 1, 1);

        var filter = Builders<TransactionItem>.Filter.Gte(x => x.DateOfDelivery, startOfYear);

        return (int)await _deliveryCollection.CountDocumentsAsync(filter);
    }

}