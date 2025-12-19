using System.Net.Quic;

namespace Lithium.Server.Core;

public sealed class Client : Component
{
    public int ServerId { get; internal set; }
    public QuicConnection Connection { get; internal set; } = null!;
}