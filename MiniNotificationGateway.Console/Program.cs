using MiniNotificationGateway.Console.Application.Abstractions.Commands;
using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Providers;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Application.Abstractions.Strategies;
using MiniNotificationGateway.Console.Application.Commands;
using MiniNotificationGateway.Console.Application.Events;
using MiniNotificationGateway.Console.Application.Facades;
using MiniNotificationGateway.Console.Application.Factories;
using MiniNotificationGateway.Console.Application.Providers;
using MiniNotificationGateway.Console.Application.Strategies;
using MiniNotificationGateway.Console.Infrastructure.Logging;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;
using MiniNotificationGateway.Console.Infrastructure.Security;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 10 Facade Pattern completed.");
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

ISendingStrategy sendingStrategy = new FailoverSendingStrategy(
    firstProviderHandler: providerAHandler);

ICommandInvoker commandInvoker = new CommandInvoker();

INotificationGatewayFacade notificationGateway = new NotificationGatewayFacade(
    messageFactory: messageFactory,
    sendingStrategy: sendingStrategy,
    commandInvoker: commandInvoker,
    eventPublisher: eventPublisher);

Console.WriteLine("Creating Message...");

var response = await notificationGateway.SendOtpAsync("09123456789");

Console.WriteLine();
Console.WriteLine("Message Information:");
Console.WriteLine($"Message Id: {response.Message.MessageId}");
Console.WriteLine($"Recipient: {response.Message.Recipient}");
Console.WriteLine($"Content: {response.Message.Content}");
Console.WriteLine($"Current Status: {response.Message.Status}");
Console.WriteLine();

Console.WriteLine($"Final Provider: {response.SendResult.ProviderName}");
Console.WriteLine($"Final Result: {response.SendResult.Description}");
Console.WriteLine($"Final Status: {response.Message.Status}");
Console.WriteLine();

Console.WriteLine("Registered Events:");

foreach (var notificationEvent in inMemoryEventObserver.Events)
{
    Console.WriteLine(
        $"- {notificationEvent.EventType} | {notificationEvent.Description} | {notificationEvent.OccurredAt:HH:mm:ss}");
}