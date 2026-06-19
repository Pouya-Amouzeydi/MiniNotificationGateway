using MiniNotificationGateway.Console.Application.Abstractions.Events;
using MiniNotificationGateway.Console.Domain.Events;

namespace MiniNotificationGateway.Console.Infrastructure.Logging;

public sealed class InMemoryEventObserver : IEventObserver
{
    private readonly List<NotificationEvent> _events = new();

    public IReadOnlyCollection<NotificationEvent> Events => _events.AsReadOnly();

    public Task OnEventAsync(NotificationEvent notificationEvent)
    {
        if (notificationEvent is null)
        {
            throw new ArgumentNullException(nameof(notificationEvent));
        }

        _events.Add(notificationEvent);

        return Task.CompletedTask;
    }
}