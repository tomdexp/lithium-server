namespace Lithium.Core.ECS;

public readonly record struct Entity(World World)
{
    public EntityId Id { get; init; }
    
    public void AddTag<T>() where T : struct, ITag
    {
        World.AddTag(this, default(T));
    }
    
    public void RemoveTag<T>() where T : struct, ITag
    {
        World.RemoveTag<T>(this);
    }
    
    public bool HasTag(Type tagType)
    {
        return World.HasTag(this, tagType);
    }
    
    public bool HasTag<T>() where T : struct, ITag
    {
        return World.HasTag<T>(this);
    }
    
    public bool HasTags(params Type[] tagTypes)
    {
        return World.HasTags(this, tagTypes);
    }
    
    public bool HasAnyTag(params Type[] tagTypes)
    {
        return World.HasAnyTag(this, tagTypes);
    }
    
    public bool HasAllTags(params Type[] tagTypes)
    {
        return World.HasAllTags(this, tagTypes);
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