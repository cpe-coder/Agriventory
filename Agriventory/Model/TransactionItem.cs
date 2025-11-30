using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class TransactionItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; } = string.Empty;
    [BsonIgnore]
    public int Number { get; set; }
    [BsonElement("customerName")]                            
    public string? CustomerName { get; set; } = string.Empty;
    [BsonElement("productName")]
    public string? ProductName { get; set; } = string.Empty;
    
    [BsonElement("quantity")]
    public int Quantity { get; set; } 
    
    [BsonElement("brand")]
    public string? Brand { get; set; } = string.Empty;
    
    [BsonElement("category")]
    public string? Category { get; set; } = string.Empty;
    
    public DateTime DateOfDelivery { get; set; }
    
    
}