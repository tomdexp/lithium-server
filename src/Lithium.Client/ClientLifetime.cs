using Lithium.Client.Core.Networking;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lithium.Client;

public sealed class ClientLifetime(
    ILogger<ClientLifetime> logger,
    QuicClient connection
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting client");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await connection.ConnectAsync(stoppingToken);
                break; // Connected successfully
            }
            catch (Exception ex)
            {
                // This is expected when running with Aspire, resources order is not guaranteed 
                // (we can make client wait for server to start, but it's whole execution won't start until server is ready, which is slower then retrying)
                logger.LogWarning("Failed to connect to server: {Message}. Retrying in 2 seconds...", ex.Message);
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}