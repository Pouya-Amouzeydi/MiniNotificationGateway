using Microsoft.Extensions.DependencyInjection;
using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.DependencyInjection;
using MiniNotificationGateway.Console.Infrastructure.Logging;
using MiniNotificationGateway.Console.Presentation.ConsoleOutput;

System.Console.WriteLine("Mini Notification Gateway");
System.Console.WriteLine("Final Demo Scenario");
System.Console.WriteLine();

var services = new ServiceCollection();

services.AddMiniNotificationGateway();

using var serviceProvider = services.BuildServiceProvider();

var notificationGateway = serviceProvider.GetRequiredService<INotificationGatewayFacade>();

var inMemoryEventObserver = serviceProvider.GetRequiredService<InMemoryEventObserver>();

var demoOutputWriter = serviceProvider.GetRequiredService<DemoOutputWriter>();

var response = await notificationGateway.SendOtpAsync("09123456789");

demoOutputWriter.Write(
    response: response,
    notificationEvents: inMemoryEventObserver.Events);