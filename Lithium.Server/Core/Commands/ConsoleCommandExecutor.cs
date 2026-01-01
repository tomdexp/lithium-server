namespace Lithium.Server.Core.Commands;

public sealed class ConsoleCommandExecutor(IServiceProvider services)
{
    public async Task ExecuteAsync(ConsoleCommand command, string[] args)
    {
        using var scope = services.CreateScope();

        var instance = scope.ServiceProvider.GetRequiredService(command.DeclaringType);

        var parameters = CommandArgumentBinder.Bind(
            command.Method.GetParameters(),
            args
        );

        if (parameters.Length != command.Method.GetParameters().Length)
            return;

        var result = command.Method.Invoke(instance, parameters);
        if (result is Task task) await task;
    }
}