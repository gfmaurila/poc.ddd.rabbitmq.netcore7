namespace Domain.Core.Events;

public abstract class BaseEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;

    protected BaseEvent()
    {

    }
}
