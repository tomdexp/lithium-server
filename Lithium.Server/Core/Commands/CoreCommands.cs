namespace Lithium.Server.Core.Commands;

public sealed class CoreCommands(
    ConsoleCommandRegistry registry
)
{
    [ConsoleCommand("help", "List available commands")]
    private void Help()
    {
        Console.WriteLine("Available commands:");
        
        foreach (var cmd in registry.Commands.Values)
            Console.WriteLine($"- {cmd.Name,-10} {cmd.Description}");
    }
}