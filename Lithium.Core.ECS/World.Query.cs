using System.Collections;

namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<ArchetypeKey, Archetype> _archetypes = [];

    // ===== Archetype Queries (O(n) direct) =====
    private IReadOnlyList<Entity> QueryArchetype(params Type[] componentTypes)
    {
        var key = new ArchetypeKey(componentTypes);

        return !_archetypes.TryGetValue(key, out var archetype)
            ? []
            : archetype.Entities;
    }

    public WorldQuery<T1> Query<T1>()
        where T1 : struct, IComponent
    {
        var entities = QueryArchetype(typeof(T1));

        var items = entities.Select(e => (e, new[]
        {
            _components[typeof(T1)].GetComponent(e)
        }));

        return new WorldQuery<T1>(items);
    }

    public WorldQuery<T1, T2> Query<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        var entities = QueryArchetype(typeof(T1), typeof(T2));

        var items = entities.Select(e => (e, new[]
        {
            _components[typeof(T1)].GetComponent(e),
            _components[typeof(T2)].GetComponent(e)
        }));

        return new WorldQuery<T1, T2>(items);
    }

    public WorldQuery<T1, T2, T3> Query<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        var entities = QueryArchetype(typeof(T1), typeof(T2), typeof(T3));

        var items = entities.Select(e => (e, new[]
        {
            _components[typeof(T1)].GetComponent(e),
            _components[typeof(T2)].GetComponent(e),
            _components[typeof(T3)].GetComponent(e)
        }));

        return new WorldQuery<T1, T2, T3>(items);
    }

    public sealed class WorldQuery<T1> :
        WorldQueryBase<WorldQuery<T1>>,
        IEnumerable<(Entity, T1)>
        where T1 : struct, IComponent
    {
        internal WorldQuery(IEnumerable<(Entity, object[])> items)
            : base(items)
        {
        }

        public IEnumerator<(Entity, T1)> GetEnumerator()
        {
            foreach (var (entity, comps) in Items)
            {
                yield return (
                    entity,
                    (T1)comps[0]
                );
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class WorldQuery<T1, T2> :
        WorldQueryBase<WorldQuery<T1, T2>>,
        IEnumerable<(Entity, T1, T2)>
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        internal WorldQuery(IEnumerable<(Entity, object[])> items)
            : base(items)
        {
        }

        public IEnumerator<(Entity, T1, T2)> GetEnumerator()
        {
            foreach (var (entity, comps) in Items)
            {
                yield return (
                    entity,
                    (T1)comps[0],
                    (T2)comps[1]
                );
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class WorldQuery<T1, T2, T3> :
        WorldQueryBase<WorldQuery<T1, T2, T3>>,
        IEnumerable<(Entity, T1, T2, T3)>
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        internal WorldQuery(IEnumerable<(Entity, object[])> items)
            : base(items)
        {
        }

        public IEnumerator<(Entity, T1, T2, T3)> GetEnumerator()
        {
            foreach (var (entity, comps) in Items)
            {
                yield return (
                    entity,
                    (T1)comps[0],
                    (T2)comps[1],
                    (T3)comps[2]
                );
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public abstract class WorldQueryBase<TSelf>(IEnumerable<(Entity, object[])> items)
        where TSelf : WorldQueryBase<TSelf>
    {
        protected IEnumerable<(Entity entity, object[] components)> Items = items;

        protected TSelf Self => (TSelf)this;

        public TSelf HasTag<TTag>() where TTag : struct, ITag
        {
            Items = Items.Where(x => x.entity.HasTag<TTag>());
            return Self;
        }

        public TSelf HasAnyTag(params Type[] tagTypes)
        {
            Items = Items.Where(x => x.entity.HasAnyTag(tagTypes));
            return Self;
        }

        public TSelf HasAllTags(params Type[] tagTypes)
        {
            Items = Items.Where(x => x.entity.HasAllTags(tagTypes));
            return Self;
        }
    }
}