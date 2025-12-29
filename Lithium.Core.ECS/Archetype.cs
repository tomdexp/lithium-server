namespace Lithium.Core.ECS;

public sealed class Archetype
{
    public readonly List<Entity> Entities = [];
    
    public static readonly Archetype Empty = new();
}