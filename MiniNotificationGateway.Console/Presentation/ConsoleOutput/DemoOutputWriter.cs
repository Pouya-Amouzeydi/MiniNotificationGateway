using MiniNotificationGateway.Console.Application.Results;
using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Presentation.ConsoleOutput;

public sealed class DemoOutputWriter
{
    public void Write(
        OtpSendResponse response,
        IReadOnlyCollection<NotificationEvent> notificationEvents)
    {
        if (response is null)
        {
            throw new ArgumentNullException(nameof(response));
        }

        if (notificationEvents is null)
        {
            throw new ArgumentNullException(nameof(notificationEvents));
        }

        System.Console.WriteLine("Creating Message...");

        var messageCreatedEvent = notificationEvents.FirstOrDefault(
            currentEvent => currentEvent.EventType == NotificationEventType.MessageCreated);

        if (messageCreatedEvent is not null)
        {
            System.Console.WriteLine(messageCreatedEvent.Description);
        }

        System.Console.WriteLine("Selecting Provider...");

        foreach (var notificationEvent in notificationEvents)
        {
            if (notificationEvent.EventType == NotificationEventType.MessageCreated)
            {
                continue;
            }

            System.Console.WriteLine(notificationEvent.Description);

            if (notificationEvent.EventType == NotificationEventType.ProviderSelected)
            {
                System.Console.WriteLine("Sending Message...");
            }
        }

        System.Console.WriteLine($"Final Status: {response.Message.Status}");
        System.Console.WriteLine();

        System.Console.WriteLine("Event Log:");

        foreach (var notificationEvent in notificationEvents)
        {
            System.Console.WriteLine(
                $"- {notificationEvent.EventType} | {notificationEvent.Description}");
        }
    }
}