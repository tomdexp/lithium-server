using System.Runtime.InteropServices;

namespace Lithium.Core.Networking.Packets;

[PacketId(10)]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct EntityPositionPacket(ulong entityId, float x, float y, float z) : IPacket
{
    public readonly ulong EntityId = entityId;
    public readonly float X = x;
    public readonly float Y = y;
    public readonly float Z = z;
}