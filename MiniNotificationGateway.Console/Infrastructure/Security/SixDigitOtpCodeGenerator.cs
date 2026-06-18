using MiniNotificationGateway.Console.Application.Abstractions.Security;

namespace MiniNotificationGateway.Console.Infrastructure.Security;

public sealed class SixDigitOtpCodeGenerator : IOtpCodeGenerator
{
    public string Generate()
    {
        return Random.Shared.Next(100000, 1000000).ToString();
    }
}