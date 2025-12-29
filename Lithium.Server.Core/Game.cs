using Lithium.Core.ECS;

namespace Lithium.Server.Core;

public class Game
{
    public World World { get; }

    public Game()
    {
        World = new World();

        var dog = World.CreateEntity();
        var cat = World.CreateEntity();

        dog.AddTag<DogTag>();

        dog.AddComponent(new Position(0, 0, 0));
        dog.AddComponent(new Rotation(0, 0, 0));

        cat.AddTag<CatTag>();

        cat.AddComponent(new Position(100, 100, 100));
        cat.AddComponent(new Rotation(100, 100, 100));
        
        var queries = World
            .Query<Position, Rotation>()
            .HasTag<DogTag>();
        
        foreach (var (entity, pos, rot) in queries)
        {
            Console.WriteLine($"{entity}: {pos} / {rot}");
        }
        
        foreach (var query in queries)
        {
            ref readonly var entity = ref query.Item1;
            ref readonly var pos = ref query.Item2;
            ref readonly var rot = ref query.Item3;
            
            Console.WriteLine($"{entity}: {pos} / {rot}");
        }
        
        queries.ForEachEntity((entity, ref pos, ref rot) =>
        {
            Console.WriteLine($"{entity}: {pos} / {rot}");
        });
    }
}