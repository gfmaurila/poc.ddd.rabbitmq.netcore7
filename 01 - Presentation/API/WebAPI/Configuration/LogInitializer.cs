using MongoDB.Bson;
using MongoDB.Driver;

namespace WebAPI.Configuration;

public class LogInitializer
{
    public static void Initialize(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
        var database = client.GetDatabase("Serilog");
        var collection = database.GetCollection<BsonDocument>("Serilog");

        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(30) };
        var fieldDefinition = new StringFieldDefinition<BsonDocument>("@t");
        var indexDefinition = new IndexKeysDefinitionBuilder<BsonDocument>().Ascending(fieldDefinition);
        collection.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(indexDefinition, indexOptions));
    }
}