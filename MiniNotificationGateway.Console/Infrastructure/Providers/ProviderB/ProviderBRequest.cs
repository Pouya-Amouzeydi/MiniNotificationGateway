namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;

public sealed class ProviderBRequest
{
    public string Destination { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;
}