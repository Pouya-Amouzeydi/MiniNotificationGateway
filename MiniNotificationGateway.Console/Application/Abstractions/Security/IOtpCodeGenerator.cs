namespace MiniNotificationGateway.Console.Application.Abstractions.Security;

public interface IOtpCodeGenerator
{
    string Generate();
}