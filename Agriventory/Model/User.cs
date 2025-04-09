using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agriventory.Model;

public class User
{
    public User(string id, string username, string password)
    {
        Id = id;
        Username = username;
        Password = password;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("password")]
    public string Password { get; set; }
}