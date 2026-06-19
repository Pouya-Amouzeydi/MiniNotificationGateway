using MiniNotificationGateway.Console.Application.Abstractions.Commands;

namespace MiniNotificationGateway.Console.Application.Commands;

public sealed class CommandInvoker : ICommandInvoker
{
    public async Task<TResult> InvokeAsync<TResult>(ICommand<TResult> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        return await command.ExecuteAsync();
    }
}