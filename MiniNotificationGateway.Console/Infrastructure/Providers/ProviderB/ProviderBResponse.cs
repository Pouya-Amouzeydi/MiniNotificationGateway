namespace MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;

public sealed class ProviderBResponse
{
    public bool Delivered { get; init; }

    public string TrackingCode { get; init; } = string.Empty;
}