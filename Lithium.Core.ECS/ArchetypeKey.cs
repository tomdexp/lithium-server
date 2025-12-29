namespace Lithium.Core.ECS;

public readonly struct ArchetypeKey : IEquatable<ArchetypeKey>
{
    private readonly int _hash;
    
    public static readonly ArchetypeKey Empty = new();

    public ArchetypeKey(ReadOnlySpan<Type> types)
    {
        unchecked
        {
            var hash = 17;

            foreach (var t in types)
            {
                int typeId;
                
                if (typeof(IComponent).IsAssignableFrom(t))
                    typeId = ComponentTypeId.GetId(t);
                else if (typeof(ITag).IsAssignableFrom(t))
                    typeId = TagTypeId.GetId(t);
                else
                    throw new ArgumentException($"Type {t.FullName} is not a component or tag");

                hash = hash * 31 + typeId;
            }

            _hash = hash;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is ArchetypeKey key && Equals(key);
    }

    public bool Equals(ArchetypeKey other) => _hash == other._hash;

    public override int GetHashCode() => _hash;

    public static bool operator ==(ArchetypeKey left, ArchetypeKey right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ArchetypeKey left, ArchetypeKey right)
    {
        return !(left == right);
    }
}