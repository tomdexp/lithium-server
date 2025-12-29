namespace Lithium.Core.ECS;

public readonly record struct Entity(World World, EntityId Id)
{
    public void AddTag<T>() where T : struct, ITag
    {
        World.AddTag<T>(this);
    }
    
    public void RemoveTag<T>() where T : struct, ITag
    {
        World.RemoveTag<T>(this);
    }
    
    public bool HasTag<T>() where T : struct, ITag
    {
        return World.HasTag<T>(this);
    }
    
    public bool HasAnyTag(ReadOnlySpan<int> tagIds)
    {
        return World.HasAnyTag(this, tagIds);
    }
    
    public bool HasAllTags(ReadOnlySpan<int> tagIds)
    {
        return World.HasAllTags(this, tagIds);
    }

    public void AddComponent<T>(T component) where T : struct, IComponent
    {
        World.AddComponent(this, component);
    }

    public bool TryGetComponent<T>(out T component) where T : struct, IComponent
    {
        return World.TryGetComponent(this, out component);
    }

    public void RemoveComponent<T>() where T : struct, IComponent
    {
        World.RemoveComponent<T>(this);
    }
}