using Domain.Contract.Producer;
using Domain.Contract.Services;

namespace Messaging.RabbitMQ.Services.Producer;

public class CreateUserAllProducer : ICreateUserAllProducer
{
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "CREATE_ALL_USER";
    public CreateUserAllProducer(IMessageBusService messageBusService)
    {
        _messageBusService = messageBusService;
    }

    public void Publish()
    {
        _messageBusService.Publish(QUEUE_NAME, null);
    }
}
