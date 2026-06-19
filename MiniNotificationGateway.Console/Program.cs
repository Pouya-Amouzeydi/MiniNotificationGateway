using Microsoft.Extensions.DependencyInjection;
using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.DependencyInjection;
using MiniNotificationGateway.Console.Infrastructure.Logging;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 13 Dependency Injection completed.");
Console.WriteLine();

var services = new ServiceCollection();

services.AddMiniNotificationGateway();

using var serviceProvider = services.BuildServiceProvider();

var notificationGateway =
    serviceProvider.GetRequiredService<INotificationGatewayFacade>();

var inMemoryEventObserver =
    serviceProvider.GetRequiredService<InMemoryEventObserver>();

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