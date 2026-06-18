using MiniNotificationGateway.Console.Domain.Messages.States;

namespace MiniNotificationGateway.Console.Domain.Messages;

public sealed class Message
{
    private IMessageState _state;

    public Guid MessageId { get; }
    public string Recipient { get; }
    public string Content { get; }
    public DateTime CreatedAt { get; }
    public MessageStatus Status { get; private set; }
    public string CurrentStateName => _state.Name;

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
        CreatedAt = DateTime.Now;
        Status = MessageStatus.Created;
        _state = new CreatedMessageState();
    }

    public void MarkAsSending()
    {
        _state.MarkAsSending(this);
    }

    public void MarkAsSent()
    {
        _state.MarkAsSent(this);
    }

    public void MarkAsFailed()
    {
        _state.MarkAsFailed(this);
    }

    internal void SetStatus(MessageStatus status)
    {
        Status = status;
    }

    internal void SetState(IMessageState state)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }
}