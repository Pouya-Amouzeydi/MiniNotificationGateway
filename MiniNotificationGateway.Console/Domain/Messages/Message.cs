namespace MiniNotificationGateway.Console.Domain.Messages;

public sealed class Message
{
    public Guid MessageId { get; }
    public string Recipient { get; }
    public string Content { get; }
    public DateTime CreatedAt { get; }
    public MessageStatus Status { get; private set; }

    public Message(string recipient, string content)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ArgumentException("Recipient cannot be empty.", nameof(recipient));
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty.", nameof(content));
        }

        MessageId = Guid.NewGuid();
        Recipient = recipient;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        Status = MessageStatus.Created;
    }

    public void MarkAsSending()
    {
        Status = MessageStatus.Sending;
    }

    public void MarkAsSent()
    {
        Status = MessageStatus.Sent;
    }

    public void MarkAsFailed()
    {
        Status = MessageStatus.Failed;
    }
}