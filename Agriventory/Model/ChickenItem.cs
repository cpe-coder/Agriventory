using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class ChickenItem
{


    public ChickenItem(int number, string productName, int stocks, string brand, DateTime dateImported)
    {
        Number = number;
        ProductName = productName;
        Stocks = stocks;
        Brand = brand;
        DateImported = dateImported;
    }

 

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Number { get; set; }
    public string ProductName { get; set; }
    public int Stocks { get; set; }
    public string Brand { get; set; }
    public DateTime DateImported { get; set; }
}