namespace MiniNotificationGateway.Console.Application.Abstractions.Commands;

public interface ICommandInvoker
{
    Task<TResult> InvokeAsync<TResult>(ICommand<TResult> command);
}