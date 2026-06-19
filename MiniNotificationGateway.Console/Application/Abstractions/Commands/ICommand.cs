namespace MiniNotificationGateway.Console.Application.Abstractions.Commands;

public interface ICommand<TResult>
{
    Task<TResult> ExecuteAsync();
}