using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestCosmo.Data;

public class Code
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }

    public string CodeString { get; set; }
}
