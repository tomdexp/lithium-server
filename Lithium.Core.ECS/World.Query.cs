using System.Runtime.CompilerServices;

namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<ArchetypeKey, Archetype> _archetypes = [];
    private readonly Dictionary<EntityId, Archetype> _entityArchetype = new();

    private Archetype GetArchetype(params Type[] componentTypes)
    {
        var key = new ArchetypeKey(componentTypes);

        return _archetypes.TryGetValue(key, out var archetype)
            ? archetype
            : Archetype.Empty;
    }

    public WorldQuery<T1, T2> Query<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        var archetype = GetArchetype(typeof(T1), typeof(T2));
        return new WorldQuery<T1, T2>(this, archetype);
    }

    public readonly struct WorldQuery<T1, T2>
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        private readonly World _world;
        private readonly Archetype _archetype;
        private readonly int[]? _with;
        private readonly int[]? _without;

        public delegate void QueryAction(Entity entity, ref T1 component1, ref T2 component2);

        internal WorldQuery(
            World world,
            Archetype archetype,
            int[]? with = null,
            int[]? without = null)
        {
            _world = world;
            _archetype = archetype;
            _with = with;
            _without = without;
        }

        public WorldQuery<T1, T2> HasTag<T>()
            where T : struct, ITag
            => new(_world, _archetype, Append(_with, TagTypeId<T>.Id), _without);

        public WorldQuery<T1, T2> WithoutTag<T>()
            where T : struct, ITag
            => new(_world, _archetype, _with, Append(_without, TagTypeId<T>.Id));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForEachEntity(QueryAction action)
        {
            var entities = _archetype.Entities;

            foreach (var entity in entities)
            {
                if (_with is not null && !_world.HasAllTags(entity, _with))
                    continue;

                if (_without is not null && _world.HasAnyTag(entity, _without))
                    continue;

                ref var c1 = ref _world.GetComponentRef<T1>(entity);
                ref var c2 = ref _world.GetComponentRef<T2>(entity);

                action(entity, ref c1, ref c2);
            }
        }

        public Enumerator GetEnumerator()
            => new(_world, _archetype, _with, _without);

        private static int[] Append(int[]? arr, int value)
        {
            if (arr is null)
                return [value];

            var copy = new int[arr.Length + 1];
            Array.Copy(arr, copy, arr.Length);
            copy[^1] = value;

            return copy;
        }

        public struct Enumerator
        {
            private readonly World _world;
            private readonly Archetype _archetype;
            private readonly int[]? _with;
            private readonly int[]? _without;
            private int _i;

            internal Enumerator(
                World world,
                Archetype archetype,
                int[]? with,
                int[]? without)
            {
                _world = world;
                _archetype = archetype;
                _with = with;
                _without = without;
                _i = -1;
            }

            public (Entity, T1, T2) Current { get; private set; }

            public bool MoveNext()
            {
                var entities = _archetype.Entities;

                while (++_i < entities.Count)
                {
                    var entity = entities[_i];

                    if (_with is not null && !_world.HasAllTags(entity, _with))
                        continue;

                    if (_without is not null && _world.HasAnyTag(entity, _without))
                        continue;

                    Current = (
                        entity,
                        _world.GetComponentRef<T1>(entity),
                        _world.GetComponentRef<T2>(entity)
                    );

                    return true;
                }

                return false;
            }
        }
    }
}