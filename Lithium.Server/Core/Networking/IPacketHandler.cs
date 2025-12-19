using Lithium.Core.Networking;

namespace Lithium.Server.Core.Networking;

public interface IPacketHandler<T>
    where T : unmanaged, IPacket
{
    void Handle(in T packet, PacketContext ctx);
}