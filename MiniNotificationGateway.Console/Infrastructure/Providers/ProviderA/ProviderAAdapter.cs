using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;
using MiniNotificationGateway.Console.Domain.Providers;

namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;

public sealed class ProviderAAdapter : INotificationProvider
{
    private readonly ProviderAClient _client;

    public ProviderAAdapter(ProviderAClient client)
    {
        _client = client;
    }

    public ProviderName Name => ProviderName.ProviderA;

    public async Task<SendResult> SendAsync(Message message)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var isSuccess = await _client.SendSmsAsync(
            phoneNumber: message.Recipient,
            text: message.Content);

        if (isSuccess)
        {
            return SendResult.Success(
                providerName: Name,
                description: "ProviderA sent the message successfully.");
        }

        return SendResult.Failure(
            providerName: Name,
            description: "ProviderA failed to send the message.");
    }
}