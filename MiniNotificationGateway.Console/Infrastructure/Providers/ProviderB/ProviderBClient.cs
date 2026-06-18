namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;

public sealed class ProviderBClient
{
    public Task<ProviderBResponse> DeliverAsync(ProviderBRequest request)
    {
        var randomNumber = Random.Shared.Next(1, 101);
        var isSuccess = randomNumber <= 90;

        var response = new ProviderBResponse
        {
            Delivered = isSuccess,
            TrackingCode = Guid.NewGuid().ToString()
        };

        return Task.FromResult(response);
    }
}