namespace Lithium.Core.ECS;

public readonly record struct Entity(World World)
{
    public EntityId Id { get; init; }
}