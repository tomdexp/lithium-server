using System.Runtime.CompilerServices;
using Lithium.Core.Networking;
using Lithium.Core.Networking.Packets;
using Microsoft.Extensions.Logging;

namespace Lithium.Server.Core.Networking;

public sealed class ClientConnectHandler(
    ILogger<ClientConnectHandler> logger,
    IClientManager clientManager,
    PacketRegistry packetRegistry
) : IPacketHandler<ClientConnectPacket>
{
    public void Handle(in ClientConnectPacket p, PacketContext ctx)
    {
        clientManager.CreateClient(ctx.Connection, p.ProtocolVersion);
        
        logger.LogInformation("Client connected: {LocalEndPoint} {RemoteEndPoint}",
            ctx.Connection.LocalEndPoint, ctx.Connection.RemoteEndPoint);

        var packet = new ServerAcceptPacket();
        var packetId = PacketRegistry.GetPacketId<ServerAcceptPacket>();
        var packetSize = (ushort)Unsafe.SizeOf<ServerAcceptPacket>();
        var header = new PacketHeader(packetId, packetSize);

        _ = Task.Run(() => ctx.Stream.WriteAsync(PacketSerializer.SerializePacket(packet, header.TypeId)));
    }
}