namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<ArchetypeKey, Archetype> _archetypes = [];
    
    // ===== Archetype Queries (O(n) direct) =====
    private IReadOnlyList<Entity> QueryArchetype(Func<ITag, bool>? tagFilter = null, params Type[] componentTypes)
    {
        var key = new ArchetypeKey(componentTypes);

        if (!_archetypes.TryGetValue(key, out var archetype))
            return [];

        if (tagFilter is null)
            return archetype.Entities;

        return archetype.Entities
            .Where(e => HasAnyTag(e, tagFilter))
            .ToArray();
    }

    // ===== Typed Wrappers =====
    public IEnumerable<(Entity, T1)> Query<T1>(Func<ITag, bool>? tagFilter = null)
        where T1 : struct, IComponent
    {
        foreach (var e in QueryArchetype(tagFilter, typeof(T1)))
        {
            TryGetComponent(e, out T1 c1);
            yield return (e, c1);
        }
    }

    public IEnumerable<(Entity, T1, T2)> Query<T1, T2>(Func<ITag, bool>? tagFilter = null)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        foreach (var e in QueryArchetype(tagFilter, typeof(T1), typeof(T2)))
        {
            TryGetComponent(e, out T1 c1);
            TryGetComponent(e, out T2 c2);
            yield return (e, c1, c2);
        }
    }

    public IEnumerable<(Entity, T1, T2, T3)> Query<T1, T2, T3>(Func<ITag, bool>? tagFilter = null)
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        foreach (var e in QueryArchetype(tagFilter, typeof(T1), typeof(T2), typeof(T3)))
        {
            TryGetComponent(e, out T1 c1);
            TryGetComponent(e, out T2 c2);
            TryGetComponent(e, out T3 c3);
            yield return (e, c1, c2, c3);
        }
    }
}