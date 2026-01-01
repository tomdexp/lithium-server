namespace Lithium.Server.Services;

public sealed class ConsoleCommandService(
    ILogger<ConsoleCommandService> logger,
    IHostApplicationLifetime lifetime,
    IServiceProvider services
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var input = Console.ReadLine();
                if (input == null) break;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                var command = parts[0].ToLower();

                switch (command)
                {
                    case "stop":
                    case "exit":
                    case "quit":
                        logger.LogInformation("Server shutdown...");
                        lifetime.StopApplication();
                        break;

                    case "help":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("  help  - Display this help");
                        Console.WriteLine("  stop  - Stop the server");
                        Console.WriteLine("  info  - Display server information");
                        break;

                    default:
                        logger.LogWarning($"Unknown command: {command}. Type 'help' for the list of commands.");
                        break;
                }
            }
        }, stoppingToken);
    }
}