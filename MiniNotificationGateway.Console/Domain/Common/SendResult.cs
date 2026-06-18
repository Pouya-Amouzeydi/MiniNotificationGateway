using MiniNotificationGateway.Console.Domain.Providers;

namespace MiniNotificationGateway.Console.Domain.Common;

public sealed class SendResult
{
    public bool IsSuccess { get; }
    public ProviderName ProviderName { get; }
    public string Description { get; }

    private SendResult(bool isSuccess, ProviderName providerName, string description)
    {
        IsSuccess = isSuccess;
        ProviderName = providerName;
        Description = description;
    }

    public static SendResult Success(ProviderName providerName, string description)
    {
        return new SendResult(true, providerName, description);
    }

    public static SendResult Failure(ProviderName providerName, string description)
    {
        return new SendResult(false, providerName, description);
    }
}