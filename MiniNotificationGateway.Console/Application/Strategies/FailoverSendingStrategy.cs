using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Application.Abstractions.Strategies;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Strategies;

public sealed class FailoverSendingStrategy : ISendingStrategy
{
    private readonly INotificationProviderHandler _firstProviderHandler;

    public FailoverSendingStrategy(INotificationProviderHandler firstProviderHandler)
    {
        _firstProviderHandler = firstProviderHandler
                                ?? throw new ArgumentNullException(nameof(firstProviderHandler));
    }

    public async Task<SendResult> SendAsync(Message message)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        return await _firstProviderHandler.HandleAsync(message);
    }
}