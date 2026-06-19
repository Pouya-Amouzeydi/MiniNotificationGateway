using MiniNotificationGateway.Console.Application.Abstractions.Logging;

namespace MiniNotificationGateway.Console.Infrastructure.Logging;

public sealed class InMemoryApplicationLogger : IApplicationLogger
{
    private readonly List<string> _logs = new();

    public IReadOnlyCollection<string> Logs => _logs.AsReadOnly();

    public void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        _logs.Add(message);
    }
}