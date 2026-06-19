using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.Application.Results;

namespace MiniNotificationGateway.Console.Application.Proxies;

public sealed class RateLimitedNotificationGatewayFacadeProxy : INotificationGatewayFacade
{
    private readonly INotificationGatewayFacade _innerGateway;
    private readonly int _maxRequestsPerRecipient;
    private readonly Dictionary<string, int> _recipientRequestCounts = new();

    public RateLimitedNotificationGatewayFacadeProxy(
        INotificationGatewayFacade innerGateway,
        int maxRequestsPerRecipient)
    {
        if (maxRequestsPerRecipient <= 0)
        {
            throw new ArgumentException(
                "Max requests per recipient must be greater than zero.",
                nameof(maxRequestsPerRecipient));
        }

        _innerGateway = innerGateway ?? throw new ArgumentNullException(nameof(innerGateway));
        _maxRequestsPerRecipient = maxRequestsPerRecipient;
    }

    public async Task<OtpSendResponse> SendOtpAsync(string recipient)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ArgumentException("Recipient cannot be empty.", nameof(recipient));
        }

        var normalizedRecipient = recipient.Trim();

        if (!_recipientRequestCounts.ContainsKey(normalizedRecipient))
        {
            _recipientRequestCounts[normalizedRecipient] = 0;
        }

        var currentRequestCount = _recipientRequestCounts[normalizedRecipient];

        if (currentRequestCount >= _maxRequestsPerRecipient)
        {
            throw new InvalidOperationException(
                $"OTP request limit exceeded for recipient {normalizedRecipient}.");
        }

        _recipientRequestCounts[normalizedRecipient] = currentRequestCount + 1;

        System.Console.WriteLine(
            $"[Rate Limit Proxy] Request allowed for {normalizedRecipient}. " +
            $"Request count: {_recipientRequestCounts[normalizedRecipient]}/{_maxRequestsPerRecipient}");

        return await _innerGateway.SendOtpAsync(normalizedRecipient);
    }
}