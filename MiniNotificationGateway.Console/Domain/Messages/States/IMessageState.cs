namespace MiniNotificationGateway.Console.Domain.Messages.States;

public interface IMessageState
{
    string Name { get; }

    void MarkAsSending(Message message);

    void MarkAsSent(Message message);

    void MarkAsFailed(Message message);
}