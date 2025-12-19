using System.Net.Quic;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;

namespace Lithium.Server.Core.Networking;

public interface IClientManager
{
    void RegisterClient(ulong clientId, QuicConnection connection);
    void UnregisterClient(ulong clientId);
}

public sealed class ClientManager(
    ILogger<ClientManager> logger
) : IClientManager
{
    private readonly Dictionary<ulong, QuicConnection> _clients = [];

    public void RegisterClient(ulong clientId, QuicConnection connection)
    {
        _clients[clientId] = connection;
        logger.LogInformation("Client {ClientId} connected", clientId);
    }

    public void UnregisterClient(ulong clientId)
    {
        _clients.Remove(clientId);
        logger.LogInformation("Client {ClientId} disconnected", clientId);
    }

    public QuicConnection GetClient(ulong clientId)
    {
        return _clients[clientId];
    }
}