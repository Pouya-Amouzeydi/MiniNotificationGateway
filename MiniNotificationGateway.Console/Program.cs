using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Application.Factories;
using MiniNotificationGateway.Console.Application.Providers;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;
using MiniNotificationGateway.Console.Infrastructure.Security;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 6 Chain of Responsibility completed.");
Console.WriteLine();

IOtpCodeGenerator otpCodeGenerator = new SixDigitOtpCodeGenerator();
IMessageFactory messageFactory = new OtpMessageFactory(otpCodeGenerator);

INotificationProvider providerA = new ProviderAAdapter(
    new ProviderAClient(() => 95));

INotificationProvider providerB = new ProviderBAdapter(
    new ProviderBClient(() => 50));

INotificationProviderHandler providerAHandler = new NotificationProviderHandler(providerA);
INotificationProviderHandler providerBHandler = new NotificationProviderHandler(providerB);

providerAHandler.SetNext(providerBHandler);

Console.WriteLine("Creating Message...");

var message = messageFactory.Create("09123456789");

Console.WriteLine("Message Created");
Console.WriteLine($"Message Id: {message.MessageId}");
Console.WriteLine($"Recipient: {message.Recipient}");
Console.WriteLine($"Content: {message.Content}");
Console.WriteLine($"Current Status: {message.Status}");
Console.WriteLine();

Console.WriteLine("Selecting Provider...");

message.MarkAsSending();

var sendResult = await providerAHandler.HandleAsync(message);

if (sendResult.IsSuccess)
{
    message.MarkAsSent();
    Console.WriteLine("Message Sent Successfully");
}
else
{
    message.MarkAsFailed();
    Console.WriteLine("Message Failed");
}

Console.WriteLine($"Final Provider: {sendResult.ProviderName}");
Console.WriteLine($"Final Result: {sendResult.Description}");
Console.WriteLine($"Final Status: {message.Status}");