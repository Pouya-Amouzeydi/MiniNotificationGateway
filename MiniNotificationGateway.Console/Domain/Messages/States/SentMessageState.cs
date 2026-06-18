namespace MiniNotificationGateway.Console.Domain.Messages.States;

public sealed class SentMessageState : IMessageState
{
    public string Name => "Sent";

    public void MarkAsSending(Message message)
    {
        throw new InvalidOperationException("Sent message cannot be sent again.");
    }

    public void MarkAsSent(Message message)
    {
        throw new InvalidOperationException("Message is already sent.");
    }

    public void MarkAsFailed(Message message)
    {
        throw new InvalidOperationException("Sent message cannot be marked as failed.");
    }
}