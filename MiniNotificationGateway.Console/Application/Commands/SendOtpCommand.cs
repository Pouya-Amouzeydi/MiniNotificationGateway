using MiniNotificationGateway.Console.Application.Abstractions.Commands;
using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Commands;

public sealed class SendOtpCommand : ICommand<SendResult>
{
    private readonly Message _message;
    private readonly INotificationProviderHandler _providerHandler;

    public SendOtpCommand(
        Message message,
        INotificationProviderHandler providerHandler)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));
        _providerHandler = providerHandler ?? throw new ArgumentNullException(nameof(providerHandler));
    }

    public async Task<SendResult> ExecuteAsync()
    {
        _message.MarkAsSending();

        var sendResult = await _providerHandler.HandleAsync(_message);

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