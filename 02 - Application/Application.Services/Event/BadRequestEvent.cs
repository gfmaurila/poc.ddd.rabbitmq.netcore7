using Domain.Contract.MongoDb;
using Domain.Contract.Services.Event;
using Domain.Core.MongoDb;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.Event;
public class BadRequestEvent : IErrorEvent
{
    public int StatusCode { get; } = StatusCodes.Status400BadRequest;
    private readonly ILogRepository _logRepository;

    public BadRequestEvent(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public void HandleEvent(HttpContext context, ILogger logger)
    {
        logger.LogInformation($"Bad request event occurred: {context.Response.StatusCode}", context.Response.StatusCode);

        var logEntry = new LogEntry
        {
            StatusCode = context.Response.StatusCode,
            Path = context.Request.Path,
            Method = context.Request.Method,
            TimeStamp = DateTime.UtcNow
        };

        _logRepository.InsertLog(logEntry);
    }
}

