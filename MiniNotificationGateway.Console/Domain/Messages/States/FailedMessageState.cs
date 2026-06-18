namespace MiniNotificationGateway.Console.Domain.Messages.States;

public sealed class FailedMessageState : IMessageState
{
    public string Name => "Failed";

    public void MarkAsSending(Message message)
    {
        throw new InvalidOperationException("Failed message cannot be sent again.");
    }

    public void MarkAsSent(Message message)
    {
        throw new InvalidOperationException("Failed message cannot be marked as sent.");
    }

    public void MarkAsFailed(Message message)
    {
        throw new InvalidOperationException("Message is already failed.");
    }
}