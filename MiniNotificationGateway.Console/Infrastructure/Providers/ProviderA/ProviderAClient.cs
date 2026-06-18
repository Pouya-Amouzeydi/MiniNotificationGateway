namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;

public sealed class ProviderAClient
{
    public Task<bool> SendSmsAsync(string phoneNumber, string text)
    {
        var randomNumber = Random.Shared.Next(1, 101);
        var isSuccess = randomNumber <= 70;

        return Task.FromResult(isSuccess);
    }
}