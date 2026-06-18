namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;

public sealed class ProviderAClient
{
    private readonly Func<int> _randomNumberFactory;

    public ProviderAClient()
        : this(() => Random.Shared.Next(1, 101))
    {
    }

    public ProviderAClient(Func<int> randomNumberFactory)
    {
        _randomNumberFactory = randomNumberFactory ?? throw new ArgumentNullException(nameof(randomNumberFactory));
    }

    public Task<bool> SendSmsAsync(string phoneNumber, string text)
    {
        var randomNumber = _randomNumberFactory();
        var isSuccess = randomNumber <= 70;

        return Task.FromResult(isSuccess);
    }
}