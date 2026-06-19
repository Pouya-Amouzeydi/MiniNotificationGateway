using MiniNotificationGateway.Console.Application.Abstractions.Commands;
using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Strategies;
using MiniNotificationGateway.Console.Application.Commands;
using MiniNotificationGateway.Console.Application.Results;
using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Application.Facades;

public sealed class NotificationGatewayFacade : INotificationGatewayFacade
{
    private readonly IMessageFactory _messageFactory;
    private readonly ISendingStrategy _sendingStrategy;
    private readonly ICommandInvoker _commandInvoker;
    private readonly INotificationEventPublisher _eventPublisher;

    public NotificationGatewayFacade(
        IMessageFactory messageFactory,
        ISendingStrategy sendingStrategy,
        ICommandInvoker commandInvoker,
        INotificationEventPublisher eventPublisher)
    {
        _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        _sendingStrategy = sendingStrategy ?? throw new ArgumentNullException(nameof(sendingStrategy));
        _commandInvoker = commandInvoker ?? throw new ArgumentNullException(nameof(commandInvoker));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    public async Task<OtpSendResponse> SendOtpAsync(string recipient)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ArgumentException("Recipient cannot be empty.", nameof(recipient));
        }

        var message = _messageFactory.Create(recipient);

        await _eventPublisher.PublishAsync(new NotificationEvent(
            eventType: NotificationEventType.MessageCreated,
            messageId: message.MessageId,
            description: "Message Created"));

        var command = new SendOtpCommand(
            message: message,
            sendingStrategy: _sendingStrategy);

        var sendResult = await _commandInvoker.InvokeAsync(command);

        return new OtpSendResponse(
            message: message,
            sendResult: sendResult);
    }
}