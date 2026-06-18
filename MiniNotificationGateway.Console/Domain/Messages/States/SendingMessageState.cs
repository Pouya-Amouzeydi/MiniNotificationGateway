namespace MiniNotificationGateway.Console.Domain.Messages.States;

public sealed class SendingMessageState : IMessageState
{
    public string Name => "Sending";

    public void MarkAsSending(Message message)
    {
        throw new InvalidOperationException("Message is already in sending state.");
    }

    public void MarkAsSent(Message message)
    {
        message.SetStatus(MessageStatus.Sent);
        message.SetState(new SentMessageState());
    }

    public void MarkAsFailed(Message message)
    {
        message.SetStatus(MessageStatus.Failed);
        message.SetState(new FailedMessageState());
    }
}