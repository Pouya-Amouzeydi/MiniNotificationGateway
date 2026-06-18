using MiniNotificationGateway.Console.Domain.Messages;

namespace MiniNotificationGateway.Console.Application.Abstractions.Factories;

public interface IMessageFactory
{
    Message Create(string recipient);
}