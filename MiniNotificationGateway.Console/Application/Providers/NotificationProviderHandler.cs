using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Events;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Providers;

public sealed class NotificationProviderHandler : INotificationProviderHandler
{
    private readonly INotificationProvider _provider;
    private readonly INotificationEventPublisher _eventPublisher;
    private INotificationProviderHandler? _nextHandler;

    public NotificationProviderHandler(
        INotificationProvider provider,
        INotificationEventPublisher eventPublisher)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    public INotificationProviderHandler SetNext(INotificationProviderHandler nextHandler)
    {
        _nextHandler = nextHandler ?? throw new ArgumentNullException(nameof(nextHandler));

        return nextHandler;
    }

    public async Task<SendResult> HandleAsync(Message message)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        await _eventPublisher.PublishAsync(new NotificationEvent(
            eventType: NotificationEventType.ProviderSelected,
            messageId: message.MessageId,
            description: $"{_provider.Name} Selected"));

        var result = await _provider.SendAsync(message);

        if (result.IsSuccess)
        {
            await _eventPublisher.PublishAsync(new NotificationEvent(
                eventType: NotificationEventType.MessageSent,
                messageId: message.MessageId,
                description: "Message Sent Successfully"));

            return result;
        }

        await _eventPublisher.PublishAsync(new NotificationEvent(
            eventType: NotificationEventType.MessageFailed,
            messageId: message.MessageId,
            description: $"{_provider.Name} Failed"));

        if (_nextHandler is null)
        {
            return result;
        }

        await _eventPublisher.PublishAsync(new NotificationEvent(
            eventType: NotificationEventType.ProviderChanged,
            messageId: message.MessageId,
            description: "Switching Provider..."));

        return await _nextHandler.HandleAsync(message);
    }
}