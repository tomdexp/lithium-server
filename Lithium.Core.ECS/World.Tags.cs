namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<int, ISparseSet> _tags = new();

    public void AddTag<T>(Entity entity) where T : struct, ITag
        => GetTagSet<T>().Add(entity, default);

    public void RemoveTag<T>(Entity entity) where T : struct, ITag
        => GetTagSet<T>().Remove(entity);

    public bool HasTag<T>(Entity entity) where T : struct, ITag
        => GetTagSet<T>().Has(entity);

    public bool HasAllTags(Entity entity, ReadOnlySpan<int> tagIds)
    {
        foreach (var t in tagIds)
        {
            if (!_tags.TryGetValue(t, out var set) || !set.Has(entity))
                return false;
        }

        return true;
    }

    public bool HasAnyTag(Entity entity, ReadOnlySpan<int> tagIds)
    {
        foreach (var t in tagIds)
        {
            if (_tags.TryGetValue(t, out var set) && set.Has(entity))
                return true;
        }

        return false;
    }
    
    public ReadOnlySpan<int> GetTags(Entity entity)
    {
        var tags = new List<int>();
        
        foreach (var t in _tags)
            if (t.Value.Has(entity))
                tags.Add(t.Key);
        
        return tags.ToArray();
    }

    private SparseSet<T> GetTagSet<T>() where T : struct, ITag
    {
        var id = TagTypeId<T>.Id;

        if (_tags.TryGetValue(id, out var set))
            return (SparseSet<T>)set;

        var created = new SparseSet<T>();
        _tags[id] = created;

        return created;
    }
}