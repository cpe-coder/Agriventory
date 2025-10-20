using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class ChickenItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("productName")]
    public string ProductName { get; set; }
    [BsonElement("stocks")]
    public int Stocks { get; set; }
    [BsonElement("brand")]
    public string Brand { get; set; }
    [BsonElement("dateImported")]
    public DateTime DateImported { get; set; }
}