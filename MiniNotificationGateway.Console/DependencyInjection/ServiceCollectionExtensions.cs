using Microsoft.Extensions.DependencyInjection;
using MiniNotificationGateway.Console.Application.Abstractions.Commands;
using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Application.Abstractions.Facades;
using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Application.Abstractions.Strategies;
using MiniNotificationGateway.Console.Application.Commands;
using MiniNotificationGateway.Console.Application.Decorators;
using MiniNotificationGateway.Console.Application.Events;
using MiniNotificationGateway.Console.Application.Facades;
using MiniNotificationGateway.Console.Application.Factories;
using MiniNotificationGateway.Console.Application.Providers;
using MiniNotificationGateway.Console.Application.Proxies;
using MiniNotificationGateway.Console.Application.Strategies;
using MiniNotificationGateway.Console.Infrastructure.Logging;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderA;
using MiniNotificationGateway.Console.Infrastructure.Providers.ProviderB;
using MiniNotificationGateway.Console.Infrastructure.Security;

namespace MiniNotificationGateway.Console.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMiniNotificationGateway(
        this IServiceCollection services)
    {
        services.AddSingleton<IOtpCodeGenerator, SixDigitOtpCodeGenerator>();
        services.AddSingleton<IMessageFactory, OtpMessageFactory>();

        services.AddSingleton<ICommandInvoker, CommandInvoker>();

        services.AddSingleton<INotificationEventPublisher, NotificationEventPublisher>();
        services.AddSingleton<ConsoleEventObserver>();
        services.AddSingleton<InMemoryEventObserver>();

        services.AddSingleton<ProviderAClient>(_ => new ProviderAClient(() => 95));

        services.AddSingleton<ProviderBClient>(_ => new ProviderBClient(() => 50));

        services.AddSingleton<ProviderAAdapter>();
        services.AddSingleton<ProviderBAdapter>();

        services.AddSingleton<ISendingStrategy>(serviceProvider =>
        {
            var eventPublisher = serviceProvider.GetRequiredService<INotificationEventPublisher>();

            var providerA = serviceProvider.GetRequiredService<ProviderAAdapter>();

            var providerB = serviceProvider.GetRequiredService<ProviderBAdapter>();

            var providerAHandler = new NotificationProviderHandler(
                provider: providerA,
                eventPublisher: eventPublisher);

            var providerBHandler = new NotificationProviderHandler(
                provider: providerB,
                eventPublisher: eventPublisher);

            providerAHandler.SetNext(providerBHandler);

            return new FailoverSendingStrategy(
                firstProviderHandler: providerAHandler);
        });

        services.AddSingleton<NotificationGatewayFacade>();

        services.AddSingleton<INotificationGatewayFacade>(serviceProvider =>
        {
            var eventPublisher = serviceProvider.GetRequiredService<INotificationEventPublisher>();

            var consoleEventObserver = serviceProvider.GetRequiredService<ConsoleEventObserver>();

            var inMemoryEventObserver = serviceProvider.GetRequiredService<InMemoryEventObserver>();

            eventPublisher.Subscribe(consoleEventObserver);
            eventPublisher.Subscribe(inMemoryEventObserver);

            var coreGateway = serviceProvider.GetRequiredService<NotificationGatewayFacade>();

            var loggingGateway = new LoggingNotificationGatewayFacadeDecorator(coreGateway);

            var rateLimitedGateway =
                new RateLimitedNotificationGatewayFacadeProxy(
                    innerGateway: loggingGateway,
                    maxRequestsPerRecipient: 3);

            return rateLimitedGateway;
        });

        return services;
    }
}