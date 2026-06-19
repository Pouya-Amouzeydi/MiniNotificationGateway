using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.Application.Results;

namespace MiniNotificationGateway.Console.Application.Decorators;

public sealed class LoggingNotificationGatewayFacadeDecorator : INotificationGatewayFacade
{
    private readonly INotificationGatewayFacade _innerGateway;

    public LoggingNotificationGatewayFacadeDecorator(INotificationGatewayFacade innerGateway)
    {
        _innerGateway = innerGateway ?? throw new ArgumentNullException(nameof(innerGateway));
    }

    public async Task<OtpSendResponse> SendOtpAsync(string recipient)
    {
        System.Console.WriteLine("[Gateway Log] OTP sending request started.");

        var startedAt = DateTime.Now;

        try
        {
            var response = await _innerGateway.SendOtpAsync(recipient);

            var finishedAt = DateTime.Now;
            var elapsedMilliseconds = (finishedAt - startedAt).TotalMilliseconds;

            System.Console.WriteLine(
                $"[Gateway Log] OTP sending request completed in {elapsedMilliseconds:0.00} ms.");

            return response;
        }
        catch (Exception exception)
        {
            System.Console.WriteLine(
                $"[Gateway Log] OTP sending request failed. Error: {exception.Message}");

            throw;
        }
    }
}