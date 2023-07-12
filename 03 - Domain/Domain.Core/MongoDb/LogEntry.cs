using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Core.MongoDb;
public class LogEntry
{
    public ObjectId Id { get; set; }
    public int StatusCode { get; set; }
    public string Path { get; set; }
    public string Method { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime TimeStamp { get; set; }
}

