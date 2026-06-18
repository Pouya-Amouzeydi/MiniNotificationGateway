namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;

public sealed class ProviderBClient
{
    private readonly Func<int> _randomNumberFactory;

    public ProviderBClient()
        : this(() => Random.Shared.Next(1, 101))
    {
    }

    public ProviderBClient(Func<int> randomNumberFactory)
    {
        _randomNumberFactory = randomNumberFactory ?? throw new ArgumentNullException(nameof(randomNumberFactory));
    }

    public Task<ProviderBResponse> DeliverAsync(ProviderBRequest request)
    {
        var randomNumber = _randomNumberFactory();
        var isSuccess = randomNumber <= 90;

        var response = new ProviderBResponse
        {
            Delivered = isSuccess,
            TrackingCode = Guid.NewGuid().ToString()
        };

        return Task.FromResult(response);
    }
}