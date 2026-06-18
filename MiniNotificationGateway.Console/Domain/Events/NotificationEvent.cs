namespace MiniNotificationGateway.Console.Domain.Events;

public sealed class NotificationEvent
{
    public Guid EventId { get; }
    public NotificationEventType EventType { get; }
    public Guid MessageId { get; }
    public string Description { get; }
    public DateTime OccurredAt { get; }

    public NotificationEvent(NotificationEventType eventType, Guid messageId, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        
        EventId = Guid.NewGuid();
        EventType = eventType;
        MessageId = messageId;
        Description = description;
        OccurredAt = DateTime.UtcNow;
    }
}