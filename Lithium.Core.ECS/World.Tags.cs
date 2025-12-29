namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<Type, ISparseSet> _tags = new();

    public void AddTag<T>(Entity entity, T tag) where T : struct, ITag
    {
        GetOrCreateTagSet<T>().Add(entity, tag);
    }

    public bool HasTag(Entity entity, Type tagType)
    {
        return _tags.TryGetValue(tagType, out var set)
               && set.Has(entity);
    }

    public bool HasTag<T>(Entity entity) where T : struct, ITag
    {
        return _tags.TryGetValue(typeof(T), out var set)
               && ((SparseSet<T>)set).Has(entity);
    }
    
    public bool HasTags(Entity entity, params Type[] tagTypes)
    {
        return tagTypes.All(tagType => HasTag(entity, tagType));
    }
    
    public bool HasAnyTag(Entity entity, params Type[] tagTypes)
    {
        return tagTypes.Any(tagType => HasTag(entity, tagType));
    }
    
    public bool HasAllTags(Entity entity, params Type[] tagTypes)
    {
        return tagTypes.All(tagType => HasTag(entity, tagType));
    }

    public void RemoveTag<T>(Entity entity) where T : struct, ITag
    {
        if (_tags.TryGetValue(typeof(T), out var set))
            ((SparseSet<T>)set).Remove(entity);
    }

    private bool HasAnyTag(Entity entity, Func<ITag, bool> filter)
    {
        foreach (var (tagType, set) in _tags)
        {
            if (!set.Has(entity)) continue;

            if (Activator.CreateInstance(tagType) is ITag tagInstance && filter(tagInstance))
                return true;
        }

        return false;
    }

    private SparseSet<T> GetOrCreateTagSet<T>() where T : struct, ITag
    {
        if (_tags.TryGetValue(typeof(T), out var obj))
            return (SparseSet<T>)obj;

        var set = new SparseSet<T>();
        _tags[typeof(T)] = set;
        return set;
    }
}