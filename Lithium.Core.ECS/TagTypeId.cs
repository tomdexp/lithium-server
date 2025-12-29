namespace Lithium.Core.ECS;

public static class TagTypeId
{
    public static int GetId(Type type)
    {
        return TagTypeRegistry.Register(type);
    }
}

public static class TagTypeId<T> where T : struct, ITag
{
    public static readonly int Id = TagTypeRegistry.Register(typeof(T));
}