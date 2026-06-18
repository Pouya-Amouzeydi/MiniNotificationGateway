using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Application.Factories;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;
using MiniNotificationGateway.Console.Infrastructure.Security;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 5 Adapter Pattern completed.");
Console.WriteLine();

IOtpCodeGenerator otpCodeGenerator = new SixDigitOtpCodeGenerator();
IMessageFactory messageFactory = new OtpMessageFactory(otpCodeGenerator);

INotificationProvider providerA = new ProviderAAdapter(new ProviderAClient());
INotificationProvider providerB = new ProviderBAdapter(new ProviderBClient());

Console.WriteLine("Creating Message...");

var message = messageFactory.Create("09123456789");

Console.WriteLine("Message Created");
Console.WriteLine($"Message Id: {message.MessageId}");
Console.WriteLine($"Recipient: {message.Recipient}");
Console.WriteLine($"Content: {message.Content}");
Console.WriteLine($"Current Status: {message.Status}");
Console.WriteLine();

Console.WriteLine("Testing ProviderA Adapter...");
var providerAResult = await providerA.SendAsync(message);

Console.WriteLine($"Provider: {providerAResult.ProviderName}");
Console.WriteLine($"Success: {providerAResult.IsSuccess}");
Console.WriteLine($"Description: {providerAResult.Description}");
Console.WriteLine();

Console.WriteLine("Testing ProviderB Adapter...");
var providerBResult = await providerB.SendAsync(message);

Console.WriteLine($"Provider: {providerBResult.ProviderName}");
Console.WriteLine($"Success: {providerBResult.IsSuccess}");
Console.WriteLine($"Description: {providerBResult.Description}");
Console.WriteLine();

Console.WriteLine("Adapter Pattern test completed.");