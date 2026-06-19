using MiniNotificationGateway.Console.Application.Abstractions.Commands;
using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Application.Commands;
using MiniNotificationGateway.Console.Application.Events;
using MiniNotificationGateway.Console.Application.Factories;
using MiniNotificationGateway.Console.Application.Providers;
using MiniNotificationGateway.Console.Domain.Events;
using MiniNotificationGateway.Console.Infrastructure.Logging;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;
using MiniNotificationGateway.Console.Infrastructure.Security;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 8 Command Pattern completed.");
Console.WriteLine();

INotificationEventPublisher eventPublisher = new NotificationEventPublisher();

var consoleEventObserver = new ConsoleEventObserver();
var inMemoryEventObserver = new InMemoryEventObserver();

eventPublisher.Subscribe(consoleEventObserver);
eventPublisher.Subscribe(inMemoryEventObserver);

IOtpCodeGenerator otpCodeGenerator = new SixDigitOtpCodeGenerator();
IMessageFactory messageFactory = new OtpMessageFactory(otpCodeGenerator);

INotificationProvider providerA = new ProviderAAdapter(
    new ProviderAClient(() => 95));

INotificationProvider providerB = new ProviderBAdapter(
    new ProviderBClient(() => 50));

INotificationProviderHandler providerAHandler = new NotificationProviderHandler(
    provider: providerA,
    eventPublisher: eventPublisher);

INotificationProviderHandler providerBHandler = new NotificationProviderHandler(
    provider: providerB,
    eventPublisher: eventPublisher);

providerAHandler.SetNext(providerBHandler);

ICommandInvoker commandInvoker = new CommandInvoker();

Console.WriteLine("Creating Message...");

var message = messageFactory.Create("09123456789");

await eventPublisher.PublishAsync(new NotificationEvent(
    eventType: NotificationEventType.MessageCreated,
    messageId: message.MessageId,
    description: "Message Created"));

Console.WriteLine();
Console.WriteLine("Message Information:");
Console.WriteLine($"Message Id: {message.MessageId}");
Console.WriteLine($"Recipient: {message.Recipient}");
Console.WriteLine($"Content: {message.Content}");
Console.WriteLine($"Current Status: {message.Status}");
Console.WriteLine();

Console.WriteLine("Selecting Provider...");

var sendOtpCommand = new SendOtpCommand(
    message: message,
    providerHandler: providerAHandler);

var sendResult = await commandInvoker.InvokeAsync(sendOtpCommand);

Console.WriteLine($"Final Provider: {sendResult.ProviderName}");
Console.WriteLine($"Final Result: {sendResult.Description}");
Console.WriteLine($"Final Status: {message.Status}");
Console.WriteLine();

Console.WriteLine("Registered Events:");

foreach (var notificationEvent in inMemoryEventObserver.Events)
{
    Console.WriteLine(
        $"- {notificationEvent.EventType} | {notificationEvent.Description} | {notificationEvent.OccurredAt:HH:mm:ss}");
}