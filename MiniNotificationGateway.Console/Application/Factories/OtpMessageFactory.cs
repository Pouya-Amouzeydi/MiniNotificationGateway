using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Factories;

public sealed class OtpMessageFactory : IMessageFactory
{
    private readonly IOtpCodeGenerator _otpCodeGenerator;

    public OtpMessageFactory(IOtpCodeGenerator otpCodeGenerator)
    {
        _otpCodeGenerator = otpCodeGenerator;
    }

    public Message Create(string recipient)
    {
        var otpCode = _otpCodeGenerator.Generate();
        var content = $"Your verification code is {otpCode}";

        return new Message(recipient, content);
    }
}