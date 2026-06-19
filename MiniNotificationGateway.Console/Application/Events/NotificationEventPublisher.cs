using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Application.Events;

public sealed class NotificationEventPublisher : INotificationEventPublisher
{
    private readonly List<IEventObserver> _observers = new();

    public void Subscribe(IEventObserver observer)
    {
        if (observer is null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        _observers.Add(observer);
    }

    public async Task PublishAsync(NotificationEvent notificationEvent)
    {
        if (notificationEvent is null)
        {
            throw new ArgumentNullException(nameof(notificationEvent));
        }

        foreach (var observer in _observers)
        {
            await observer.OnEventAsync(notificationEvent);
        }
    }
}