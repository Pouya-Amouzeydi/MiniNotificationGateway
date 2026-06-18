using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Messages;
using MiniNotificationGateway.Console.Domain.Providers;

namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;

public sealed class ProviderBAdapter : INotificationProvider
{
    private readonly ProviderBClient _client;

    public ProviderBAdapter(ProviderBClient client)
    {
        _client = client;
    }

    public ProviderName Name => ProviderName.ProviderB;

    public async Task<SendResult> SendAsync(Message message)
    {
        if (message is null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var request = new ProviderBRequest
        {
            Destination = message.Recipient,
            Body = message.Content
        };

        var response = await _client.DeliverAsync(request);

        if (response.Delivered)
        {
            return SendResult.Success(
                providerName: Name,
                description: $"ProviderB sent the message successfully. TrackingCode: {response.TrackingCode}");
        }

        return SendResult.Failure(
            providerName: Name,
            description: $"ProviderB failed to send the message. TrackingCode: {response.TrackingCode}");
    }
}