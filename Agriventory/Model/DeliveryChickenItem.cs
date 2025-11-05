using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class DeliveryChickenItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; } = string.Empty;
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
    [BsonElement("dateOfDelivery")]
    public DateTime DateOfDelivery { get; set; }
}