using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Results;

public sealed class OtpSendResponse
{
    public Message Message { get; }

    public SendResult SendResult { get; }

    public OtpSendResponse(Message message, SendResult sendResult)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        SendResult = sendResult ?? throw new ArgumentNullException(nameof(sendResult));
    }
}