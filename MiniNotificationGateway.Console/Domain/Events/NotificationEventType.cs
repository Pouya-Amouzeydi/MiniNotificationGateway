namespace MiniNotificationGateway.Console.Domain.Events;

public enum NotificationEventType
{
    MessageCreated = 1,
    ProviderSelected = 2,
    MessageSent = 3,
    MessageFailed = 4,
    ProviderChanged = 5
}