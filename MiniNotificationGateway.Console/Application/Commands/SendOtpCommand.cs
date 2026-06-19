using MiniNotificationGateway.Console.Application.Abstractions.Commands;
using MiniNotificationGateway.Console.Application.Abstractions.Strategies;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Commands;

public sealed class SendOtpCommand : ICommand<SendResult>
{
    private readonly Message _message;
    private readonly ISendingStrategy _sendingStrategy;

    public SendOtpCommand(
        Message message,
        ISendingStrategy sendingStrategy)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));
        _sendingStrategy = sendingStrategy ?? throw new ArgumentNullException(nameof(sendingStrategy));
    }

    public async Task<SendResult> ExecuteAsync()
    {
        _message.MarkAsSending();

        var sendResult = await _sendingStrategy.SendAsync(_message);

        if (sendResult.IsSuccess)
        {
            _message.MarkAsSent();
        }
        else
        {
            _message.MarkAsFailed();
        }

        return sendResult;
    }
}