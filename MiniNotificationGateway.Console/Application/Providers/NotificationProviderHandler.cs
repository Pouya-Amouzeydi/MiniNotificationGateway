using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Providers;

public sealed class NotificationProviderHandler : INotificationProviderHandler
{
    private readonly INotificationProvider _provider;
    private INotificationProviderHandler? _nextHandler;

    public NotificationProviderHandler(INotificationProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
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

        System.Console.WriteLine($"{_provider.Name} Selected");
        System.Console.WriteLine("Sending Message...");

        var result = await _provider.SendAsync(message);

        if (result.IsSuccess)
        {
            return result;
        }

        System.Console.WriteLine($"{_provider.Name} Failed");

        if (_nextHandler is null)
        {
            return result;
        }

        System.Console.WriteLine("Switching Provider...");

        return await _nextHandler.HandleAsync(message);
    }
}