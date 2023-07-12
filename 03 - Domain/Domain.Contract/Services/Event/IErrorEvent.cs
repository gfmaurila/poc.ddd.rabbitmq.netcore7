using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Domain.Contract.Services.Event;
public interface IErrorEvent
{
    int StatusCode { get; }
    void HandleEvent(HttpContext context, ILogger logger);
}