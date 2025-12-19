using System.Net.Quic;

namespace Lithium.Server.Core.Networking;

public readonly struct PacketContext(QuicConnection connection)
{
    public readonly QuicConnection Connection = connection;
}