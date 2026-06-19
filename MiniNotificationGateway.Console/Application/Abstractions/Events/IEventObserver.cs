using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Application.Abstractions.Events;

public interface IEventObserver
{
    Task OnEventAsync(NotificationEvent notificationEvent);
}