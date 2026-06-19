using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.Application.Abstractions.Logging;
using MiniNotificationGateway.Console.Application.Results;

namespace MiniNotificationGateway.Console.Application.Decorators;

public sealed class LoggingNotificationGatewayFacadeDecorator : INotificationGatewayFacade
{
    private readonly INotificationGatewayFacade _innerGateway;
    private readonly IApplicationLogger _logger;

    public LoggingNotificationGatewayFacadeDecorator(
        INotificationGatewayFacade innerGateway,
        IApplicationLogger logger)
    {
        _innerGateway = innerGateway ?? throw new ArgumentNullException(nameof(innerGateway));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<OtpSendResponse> SendOtpAsync(string recipient)
    {
        _logger.Log("[Gateway Log] OTP sending request started.");

        var startedAt = DateTime.Now;

        try
        {
            var response = await _innerGateway.SendOtpAsync(recipient);

            var finishedAt = DateTime.Now;
            var elapsedMilliseconds = (finishedAt - startedAt).TotalMilliseconds;

            _logger.Log(
                $"[Gateway Log] OTP sending request completed in {elapsedMilliseconds:0.00} ms.");

            return response;
        }
        catch (Exception exception)
        {
            _logger.Log(
                $"[Gateway Log] OTP sending request failed. Error: {exception.Message}");

            throw;
        }
    }
}