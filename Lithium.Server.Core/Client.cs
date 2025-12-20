using System.Net.Quic;

namespace Lithium.Server.Core;

public sealed class Client(QuicConnection connection, int protocolVersion, int serverId)
{
    public readonly QuicConnection Connection = connection;
    public readonly int ProtocolVersion = protocolVersion;
    public readonly int ServerId = serverId;
}