using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Application.Abstractions.Events;

public interface INotificationEventPublisher
{
    void Subscribe(IEventObserver observer);

    Task PublishAsync(NotificationEvent notificationEvent);
}