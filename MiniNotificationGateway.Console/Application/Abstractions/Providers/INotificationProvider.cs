using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;
using MiniNotificationGateway.Console.Domain.Providers;

namespace MiniNotificationGateway.Console.Application.Abstractions.Providers;

public interface INotificationProvider
{
    ProviderName Name { get; }

    Task<SendResult> SendAsync(Message message);
}