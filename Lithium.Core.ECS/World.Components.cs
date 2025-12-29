using System.Runtime.CompilerServices;

namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<Type, ISparseSet> _components = new();

    public void AddComponent<T>(Entity e, T component)
        where T : struct, IComponent
        => GetSet<T>().Add(e, component);

    public void RemoveComponent<T>(Entity e)
        where T : struct, IComponent
        => GetSet<T>().Remove(e);

    public bool TryGetComponent<T>(Entity e, out T component)
        where T : struct, IComponent
        => GetSet<T>().TryGet(e, out component);

    private SparseSet<T> GetSet<T>() where T : struct
    {
        if (_components.TryGetValue(typeof(T), out var set))
            return (SparseSet<T>)set;

        var created = new SparseSet<T>();
        _components[typeof(T)] = created;
        
        return created;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponentRef<T>(Entity entity)
        where T : struct, IComponent
    {
        return ref ((SparseSet<T>)_components[typeof(T)]).GetComponentRef(entity);
    }
}