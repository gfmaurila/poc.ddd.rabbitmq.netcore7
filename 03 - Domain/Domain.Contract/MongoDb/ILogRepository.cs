using Domain.Core.MongoDb;

namespace Domain.Contract.MongoDb;

public interface ILogRepository
{
    void InsertLog(LogEntry logEntry);
}

