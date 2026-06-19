using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Abstractions.Strategies;

public interface ISendingStrategy
{
    Task<SendResult> SendAsync(Message message);
}