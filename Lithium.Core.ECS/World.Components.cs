namespace Lithium.Core.ECS;

public partial class World
{
    private readonly Dictionary<Type, ISparseSet> _components = new();

    private SparseSet<T> GetOrCreateSet<T>() where T : struct, IComponent
    {
        if (_components.TryGetValue(typeof(T), out var obj))
            return (SparseSet<T>)obj;

        var set = new SparseSet<T>();
        _components[typeof(T)] = set;
        
        return set;
    }

    private ISparseSet GetOrCreateSetByType(Type type)
    {
        if (_components.TryGetValue(type, out var obj))
            return obj;

        var setType = typeof(SparseSet<>).MakeGenericType(type);
        var set = (ISparseSet)Activator.CreateInstance(setType)!;
        _components[type] = set;
        return set;
    }

    public bool TryGetComponent<T>(Entity entity, out T component)
        where T : struct, IComponent
    {
        if (_components.TryGetValue(typeof(T), out var obj))
            return ((SparseSet<T>)obj).TryGet(entity, out component);

        component = default;
        return false;
    }
}