using MiniNotificationGateway.Console.Application.Results;

namespace MiniNotificationGateway.Console.Application.Abstractions.Facades;

public interface INotificationGatewayFacade
{
    Task<OtpSendResponse> SendOtpAsync(string recipient);
}