namespace MiniNotificationGateway.Console.Domain.Messages;

public enum MessageStatus
{
    Created = 1,
    Sending = 2,
    Sent = 3,
    Failed = 4
}