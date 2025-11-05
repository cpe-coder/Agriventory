using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class TransactionItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; } = string.Empty;
    [BsonElement("quantity")]
    public int Number { get; set; }
    [BsonElement("customerName")]
    public string? CustomerName { get; set; }
    [BsonElement("productName")]
    public string? ProductName { get; set; } = string.Empty;
    [BsonElement("quantity")]
    public string? Quantity { get; set; }
    [BsonElement("category")]
    public string? Category { get; set; }
    [BsonElement("brand")]
    public string? Brand { get; set; }
    [BsonElement("dateOfDelivery")]
    public DateTime DateOfDelivery { get; set; }
}