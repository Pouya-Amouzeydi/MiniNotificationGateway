using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Abstractions.Providers;

public interface INotificationProviderHandler
{
    INotificationProviderHandler SetNext(INotificationProviderHandler nextHandler);

    Task<SendResult> HandleAsync(Message message);
}