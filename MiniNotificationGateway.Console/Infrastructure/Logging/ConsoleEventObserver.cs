using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Infrastructure.Logging;

public sealed class ConsoleEventObserver : IEventObserver
{
    public Task OnEventAsync(NotificationEvent notificationEvent)
    {
        if (notificationEvent is null)
        {
            throw new ArgumentNullException(nameof(notificationEvent));
        }

        System.Console.WriteLine(notificationEvent.Description);

        if (notificationEvent.EventType == NotificationEventType.ProviderSelected)
        {
            System.Console.WriteLine("Sending Message...");
        }

        return Task.CompletedTask;
    }
}