using Domain.Contract.MongoDb;
using Domain.Core.MongoDb;
using MongoDB.Driver;

namespace Data.Repository.MongoDb;
public class LogRepository : ILogRepository
{
    private readonly IMongoCollection<LogEntry> _logCollection;

    public LogRepository(IMongoClient client)
    {
        var database = client.GetDatabase("ErrorLogDb");
        _logCollection = database.GetCollection<LogEntry>("LogEntries");

        // Crie o índice TTL no campo TimeStamp. 
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(30) };
        var fieldDefinition = new StringFieldDefinition<LogEntry>("TimeStamp");
        var indexDefinition = new IndexKeysDefinitionBuilder<LogEntry>().Ascending(fieldDefinition);
        _logCollection.Indexes.CreateOne(new CreateIndexModel<LogEntry>(indexDefinition, indexOptions));
    }

    public void InsertLog(LogEntry logEntry)
    {
        _logCollection.InsertOne(logEntry);
    }
}

