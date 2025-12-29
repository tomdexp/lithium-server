namespace Lithium.Core.ECS;

public partial class World
{
    private uint _nextEntityId;

    public Entity CreateEntity()
    {
        var entity = new Entity(this, ++_nextEntityId);
        var key = ArchetypeKey.Empty;
        
        if (!_archetypes.TryGetValue(key, out var archetype))
            _archetypes[key] = archetype = new Archetype();

        archetype.Entities.Add(entity);
        _entityArchetype[entity.Id] = archetype;

        return entity;
    }
}