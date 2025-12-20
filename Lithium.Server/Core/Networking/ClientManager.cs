using System.Net.Quic;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;

namespace Lithium.Server.Core.Networking;

public interface IClientManager
{
    void CreateClient(QuicConnection connection, int protocolVersion);
    void RemoveClient(QuicConnection connection);
    Client GetClient(QuicConnection connection);
}

public sealed class ClientManager(
    ILogger<ClientManager> logger
) : IClientManager
{
    private int _currentServerId;
    private readonly Dictionary<QuicConnection, Client> _clients = [];

    public void CreateClient(QuicConnection connection, int protocolVersion)
    {
        var serverId = GetNextServerId();

        _clients[connection] = new Client(connection, protocolVersion, serverId);
        logger.LogInformation($"Client connected using protocol version {protocolVersion} with server id {serverId}");
    }

    public void RemoveClient(QuicConnection connection)
    {
        _clients.Remove(connection);
        logger.LogInformation("Client {ClientId} disconnected", connection);
    }

    public Client GetClient(QuicConnection connection)
    {
        return _clients[connection];
    }

    private int GetNextServerId() => _currentServerId is 0 ? 0 : _currentServerId++;
}