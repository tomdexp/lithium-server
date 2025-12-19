using System.Numerics;

namespace Lithium.Server.Core;

public abstract class Entity
{
    public int Id { get; set; }
    public Vector3 Position { get; internal set; }
}