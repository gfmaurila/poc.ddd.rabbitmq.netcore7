using Application.Services.Event;
using Domain.Contract.MongoDb;
using Domain.Contract.Services.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.Middleware;

public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;
    private readonly ILogRepository _logRepository;

    private Dictionary<int, IErrorEvent> _errorEvents;

    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger, ILogRepository logRepository)
    {
        _next = next;
        _logger = logger;
        _logRepository = logRepository;

        _errorEvents = new Dictionary<int, IErrorEvent>
        {
            { StatusCodes.Status400BadRequest, new BadRequestEvent(_logRepository) },
            { StatusCodes.Status404NotFound, new NotFoundEvent(_logRepository) },
            { StatusCodes.Status500InternalServerError, new InternalServerErrorEvent(_logRepository) }
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (_errorEvents.ContainsKey(context.Response.StatusCode))
        {
            var errorEvent = _errorEvents[context.Response.StatusCode];
            errorEvent.HandleEvent(context, _logger);
        }
    }
}



