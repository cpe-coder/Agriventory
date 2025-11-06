using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class ChickenItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; } = string.Empty;
    [BsonIgnore]
    public int Number { get; set; }
    [BsonElement("productName")]
    public string? ProductName { get; set; } = string.Empty;
    [BsonElement("stocks")]
    public int Stocks { get; set; } 
    [BsonElement("brand")]
    public string? Brand { get; set; } = string.Empty;
    [BsonElement("dateImported")]
    public DateTime DateImported { get; set; }
    
    [BsonElement("dateUpdated")]
    public DateTime DateUpdated { get; set; }
     
}