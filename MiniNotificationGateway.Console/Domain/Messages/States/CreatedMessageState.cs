namespace MiniNotificationGateway.Console.Domain.Messages.States;

public sealed class CreatedMessageState : IMessageState
{
    public string Name => "Created";

    public void MarkAsSending(Message message)
    {
        message.SetStatus(MessageStatus.Sending);
        message.SetState(new SendingMessageState());
    }

    public void MarkAsSent(Message message)
    {
        throw new InvalidOperationException("Message cannot be marked as sent before sending starts.");
    }

    public void MarkAsFailed(Message message)
    {
        throw new InvalidOperationException("Message cannot be marked as failed before sending starts.");
    }
}